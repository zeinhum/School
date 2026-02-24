// ===========================
// CONFIG
// ===========================
const FALLBACK_LOGO = "/image/logo.png";
const DB_NAME = "MediaDB";
const DB_VERSION = 3;
const IMAGE_STORE_NAME = "image_notes";
const LOGO_KEY = "logo";

// ===========================
// MAIN ENTRY
// ===========================
export async function writeHeader(data) {
  try {
    
    // 2. Save session if provided
    if (data && data.user) {
      saveSession(data);

      // Fetch logo in background (never blocks UI)
      if (data.logoPath) {
        fetchAndSaveLogo(data.logoPath);
      }
    }

    // 3. Try enhancing logo (safe async)
    const logoUrl = await safeLoadLogo();
    if (logoUrl) {
      updateLogo(logoUrl);
    }

    renderBasicHeader();
  } catch (e) {
    console.error("Header init failed, fallback used", e);
  }
}

// ===========================
// BASIC FALLBACK HEADER
// ===========================
function renderBasicHeader() {
  const container = document.querySelector(".dashboard-header");
  if (!container) return;

  const session = getSession();

  container.innerHTML = `
    <div class="logo">
      <img src="${FALLBACK_LOGO}" height="30" alt="Logo">
      <h3>${session.school}</h3>
    </div>
    <div class="user">
      <p data-userid="${session.userId}">${session.user}</p>
      <a class="login" href="/Login/Logout">Logout</a>
    </div>
  `;
}

// ===========================
// SESSION HELPERS
// ===========================
function saveSession(data) {
  sessionStorage.setItem("schoolUser", data.user);
  sessionStorage.setItem("schoolName", data.school);
  sessionStorage.setItem("userId", data.userId);
}

function getSession() {
  return {
    user: sessionStorage.getItem("schoolUser") || "You",
    school: sessionStorage.getItem("schoolName") || "School",
    userId: sessionStorage.getItem("userId") || "id"
  };
}

// ===========================
// UPDATE ONLY LOGO
// ===========================
function updateLogo(url) {
  const img = document.querySelector(".dashboard-header img");
  if (img) img.src = url;
}

// ===========================
// FETCH + SAVE LOGO (ASYNC, SAFE)
// ===========================
async function fetchAndSaveLogo(path) {
  if (!window.indexedDB) return;

  try {
    const resp = await fetch(path, { cache: "force-cache" });
    if (!resp.ok) return;

    const blob = await resp.blob();
    if (blob.size > 500000) return; // skip large files

    await saveToDB(blob);
  } catch (_) {
    // silent fail
  }
}

// ===========================
// INDEXEDDB CORE
// ===========================
function openDB() {
  return new Promise((resolve, reject) => {
    const req = indexedDB.open(DB_NAME, DB_VERSION);

    req.onupgradeneeded = (e) => {
      const db = e.target.result;
      if (!db.objectStoreNames.contains(IMAGE_STORE_NAME)) {
        db.createObjectStore(IMAGE_STORE_NAME, { keyPath: "id" });
      }
    };

    req.onsuccess = (e) => resolve(e.target.result);
    req.onerror = () => reject();
  });
}

// ===========================
// SAVE LOGO
// ===========================
async function saveToDB(blob) {
  try {
    const db = await openDB();

    if (!db.objectStoreNames.contains(IMAGE_STORE_NAME)) {
      db.close();
      return;
    }

    const tx = db.transaction(IMAGE_STORE_NAME, "readwrite");
    const store = tx.objectStore(IMAGE_STORE_NAME);

    store.put({ id: LOGO_KEY, data: blob });

    tx.oncomplete = () => db.close();
    tx.onerror = () => db.close();
  } catch (_) {}
}

// ===========================
// LOAD LOGO (SAFE)
// ===========================
async function safeLoadLogo() {
  if (!window.indexedDB) return null;

  try {
    const db = await openDB();

    if (!db.objectStoreNames.contains(IMAGE_STORE_NAME)) {
      db.close();
      return null;
    }

    return await new Promise((resolve) => {
      const tx = db.transaction(IMAGE_STORE_NAME, "readonly");
      const store = tx.objectStore(IMAGE_STORE_NAME);
      const req = store.get(LOGO_KEY);

      req.onsuccess = () => {
        const result = req.result;
        db.close();

        if (!result || !result.data) return resolve(null);
        resolve(URL.createObjectURL(result.data));
      };

      req.onerror = () => {
        db.close();
        resolve(null);
      };
    });
  } catch {
    return null;
  }
}
