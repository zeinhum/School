import { initializer } from "./_fillMPartial.js";
import { Attendance } from "./attendence.js";

class TableActionHandler {
  constructor(tableSelector, partialContainerSelector) {
    this.table = document.querySelector(tableSelector);
    this.partialContainer = document.querySelector(partialContainerSelector);

    if (!this.table) {
      console.warn(`⚠️ Table not found: ${tableSelector}`);
      return;
    }

    this.init();
  }

  init() {
    // Listen for changes on the table
    this.table.addEventListener("change", (e) => {
      this.handleChange(e);
    });
  }

  handleChange(e) {
    const select = e.target.closest("select.select-option");
    if (!select) return;

    const selectedOption = select.options[select.selectedIndex];
    const action = selectedOption.getAttribute("data-action");

    if (!action || action === "no") return;

    const row = select.closest("tr");
    const rowData = this.getRowData(row, selectedOption);

    this.displayHtml(action, rowData);
  }

  getRowData(row, selectedOption) {
    const data = {};

    // Grab cell values
    Array.from(row.cells).forEach((cell) => {
      const name = cell.getAttribute("name");
      if (name) {
        let value = cell.getAttribute("value") ?? cell.innerText;
        if (name === "ClassId") value = parseInt(value, 10);
        data[name] = value;
      }
    });

    // Include selected option's own name/value
    const optName = selectedOption.getAttribute("name");
    const optValue = selectedOption.getAttribute("value");
    if (optName) data[optName] = parseInt(optValue, 10);

    return data;
  }

  // send data and populate partial html form
  displayHtml(action, data) {
    const url = `/Teachers/TeachersDashboard/${action}`;

    fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    })
      .then((res) => {
        if (!res.ok) throw new Error("Network error");
        return res.text();
      })
      .then((html) => {
        if (this.partialContainer) {
          this.partialContainer.innerHTML = html;

          if (action === "AttendenceReq") {
            const attendence= new Attendance();
            attendence.SubmitAddendence("/Teachers/TeachersDashboard/MarkStudsAttendence");
          } else {
            initializer();
          }
        } else {
          console.warn("⚠️ No container found for partial content.");
        }
      })
      .catch((err) => console.error("❌ Fetch error:", err));
  }
}

// Initialize the class when DOM is ready
document.addEventListener("DOMContentLoaded", () => {
  new TableActionHandler(".data-table", ".partial-container");
});
