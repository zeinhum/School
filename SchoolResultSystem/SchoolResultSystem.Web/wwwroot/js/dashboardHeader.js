// ---------------------------
// Main function to write header
// ---------------------------
export async function writeHeader(data = null) {
  const hcontainer = document.querySelector(".dashboard-header");

  // If user data comes from server
  if (data?.user) {
    sessionStorage.setItem("schoolUser", data.user);
    sessionStorage.setItem("schoolName", data.school);

    if (data.logoPath) {
      await fetchAndSaveLogo(data.logoPath); // fetch logo and store in IndexedDB
    }
  }

  // Get user and school from sessionStorage
  const [user, school] = checkSession();

  // Load logo from IndexedDB
  const logo = await loadLogo();

  hcontainer.innerHTML = `
    <div class='logo'>
      <img src="${logo}" alt="Logo" style="height:40px;">
      <h3>${school}</h3>
    </div>
    <div class="user">
      <p>${user}</p>
      <a class="login" href="/Login/Logout">Logout</a>
    </div>`;
}

// ---------------------------
// IndexedDB helpers
// ---------------------------
const DB_NAME = 'MediaDB';
const DB_VERSION = 2;
const IMAGE_STORE_NAME = 'image_notes';

function saveToDB(logoData) {
  const request = indexedDB.open(DB_NAME, DB_VERSION);

  request.onupgradeneeded = (event) => {
    const db = event.target.result;
    if (!db.objectStoreNames.contains(IMAGE_STORE_NAME)) {
      db.createObjectStore(IMAGE_STORE_NAME, { keyPath: 'id' });
    }
  };

  request.onsuccess = (event) => {
    const db = event.target.result;
    const tx = db.transaction(IMAGE_STORE_NAME, 'readwrite');
    const store = tx.objectStore(IMAGE_STORE_NAME);

    store.put({ id: 'logo', data: logoData });

    tx.oncomplete = () => console.log("Logo saved to IndexedDB!");
    tx.onerror = (e) => console.error("Error saving logo:", e);
  };

  request.onerror = (e) => console.error("IndexedDB open error:", e);
}

function loadLogo() {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(DB_NAME, DB_VERSION);

    request.onsuccess = (event) => {
      const db = event.target.result;
      const tx = db.transaction(IMAGE_STORE_NAME, 'readonly');
      const store = tx.objectStore(IMAGE_STORE_NAME);

      const getReq = store.get('logo');
      getReq.onsuccess = () => resolve(getReq.result ? getReq.result.data : null);
      getReq.onerror = () => reject("Error loading logo");
    };

    request.onerror = () => reject("IndexedDB open error");
  });
}

// ---------------------------
// Fetch logo from server and save
// ---------------------------
async function fetchAndSaveLogo(path) {
  try {
    const resp = await fetch(path);
    if (!resp.ok) throw new Error(`Failed to fetch logo: ${resp.status}`);

    const blob = await resp.blob();
    const reader = new FileReader();

    reader.onloadend = () => saveToDB(reader.result);
    reader.readAsDataURL(blob);
  } catch (err) {
    console.error(err);
  }
}

// ---------------------------
// Session helpers
// ---------------------------
function checkSession() {
  const user = sessionStorage.getItem("schoolUser") || "You";
  const school = sessionStorage.getItem("schoolName") || "School";
  return [user, school];
}
