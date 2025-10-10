import { initializer } from "./_fillMPartial.js";

document.addEventListener("DOMContentLoaded", function () {
  const table = document.querySelector(".data-table");
  

  table.addEventListener("click", function (e) {
    // Find the closest button with a data-action attribute
    const button = e.target.closest("button[data-action]");
    if (!button) return;

    const action = button.getAttribute("data-action");

    // Prepare object to hold row data
    const rowData = {};

    Array.from(button.closest("tr").cells).forEach((cell) => {
      const name = cell.getAttribute("name");
      if (name) {
        let value = cell.getAttribute("value") ?? cell.innerText;

        // Convert ClassId to integer
        if (name === "ClassId") {
          value = parseInt(value, 10);
        }

        rowData[name] = value;
      }
    });

    // 2️⃣ Extract button's own name and value
    const btnName = button.getAttribute("name");
    const btnValue = button.getAttribute("value");
    if (btnName) {
      rowData[btnName] = parseInt(btnValue, 10);
    }

    sendData(action, rowData);
  });
});

function sendData(baseUrl, data) {
  const url = `/Teachers/TeachersDashboard/${baseUrl}`;
  console.log(`data being sent: ${JSON.stringify(data)}`);

  fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  })
    // Because PartialView returns HTML, not JSON
    .then((res) => {
      if (!res.ok) throw new Error("Network error");
      return res.text(); // <-- get raw HTML instead of JSON
    })
    .then((html) => {
      console.log("📦 Partial view received.");
        const partial = document.querySelector(".partial-container");
      if (partial) {
        partial.innerHTML = html; // <-- inject the partial HTML here
        initializer(); //partial
      } else {
        console.warn("⚠️ No container found for partial content.");
      }
    })
    .catch((err) => console.error("❌ Fetch error:", err));
}
