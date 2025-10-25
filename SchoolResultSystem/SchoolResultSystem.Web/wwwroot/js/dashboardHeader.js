export function writeHeader(data = null) {
  const hcontainer = document.querySelector(".dashboard-header");
  if (data.user) {
    sessionStorage.setItem("schoolUser", data.user);
    sessionStorage.setItem("schoolName", data.school);
  }
  const [user, school] = chekSession();
  hcontainer.innerHTML = `<div>
        <h3>${school}</h3>
    </div>
    <div class="user">
    <p>${user}</p>
    <a class="login" href="/Login/Logout">Logout</a></div>`;
}

function chekSession() {
  const user = sessionStorage.getItem("schoolUser");
  const school = sessionStorage.getItem("schoolName");

  if (user && school) {
    return [user, school];
  } else {
      return ["You", "School"];
    }
  }

