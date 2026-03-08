export class TableRowDuplicate {
  constructor(tbodySelector = ".form-data") {
    this.tbody = document.querySelector(tbodySelector);
    if (!this.tbody) return;

    // 1. Initial setup for the first row
    const firstRow = this.tbody.querySelector("tr");
    if (firstRow) this.ensureControlButtons(firstRow);

    // 2. Single listener for ALL current and future buttons
    this.handleTableClick = this.handleTableClick.bind(this);
    this.tbody.addEventListener("click", this.handleTableClick);
  }

  handleTableClick(e) {
    const target = e.target;
    const row = target.closest("tr");

    if (target.classList.contains("btn-add")) {
      this.replicateRow(row);
    } else if (target.classList.contains("btn-remove")) {
      this.removeRow(row);
    }
  }

  replicateRow(templateRow) {
    const newRow = templateRow.cloneNode(true);

    // Reset inputs
    newRow.querySelectorAll("input").forEach((i) => (i.value = ""));
    newRow.querySelectorAll("select").forEach((s) => (s.selectedIndex = 0));

    this.tbody.appendChild(newRow);
  }

  removeRow(row) {
    if (this.tbody.querySelectorAll("tr").length > 1) {
      row.remove();
    }
  }

  ensureControlButtons(row) {
    if (row.querySelector(".btn-group")) return;

    const td = document.createElement("td");
    td.className = "btn-group";
    // We use classes now instead of direct .onclick
    td.innerHTML = `
      <button type="button" class="btn-remove">-</button>
      <button type="button" class="btn-add">+</button>
    `;
    row.appendChild(td);
  }

  // CRITICAL for Garbage Collection
  destroy() {
    this.tbody.removeEventListener("click", this.handleTableClick);
    this.tbody = null;
  }
}

export class RowDataCollector {
  constructor(tbodySelector = ".form-data", errorSelector = ".error") {
    this.tbodySelector = tbodySelector;
  }

  getData() {
    const rows = document.querySelectorAll(`${this.tbodySelector} tr`);
    let formData = [];
    let hasError = false;

    for (const [index, row] of rows.entries()) {
      const fields = row.querySelectorAll("input, select, textarea");
      const rowData = {};

      for (const field of fields) {
        const name = field.hasAttribute("name")
          ? field.getAttribute("name").trim()
          : null;

        const value = field.value?.trim();

        if (!value) {
          alert(`some fields are empty \n ${index+1}`);
          hasError = true;
          return false;
        }

        // check name
        if (!name) {
          continue;
        } else if (field.type === "number") {
          rowData[name] = parseFloat(value);
        } else {
          rowData[name] = value;
        }
      }

      if (hasError) return false;

      if (rowData.ClassId) rowData.ClassId = parseInt(rowData.ClassId, 10);
      formData.push(rowData);
    }

    return formData;
  }
}

export class FormSubmitter {
  constructor(formSelector = ".form-container") {
    this.form = document.querySelector(formSelector);
  }

  async submit(data) {
    const actionUrl = this.form.action;

    try {
      const res = await fetch(actionUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      const resp = await res.json();
      
      if (resp.success) {
        alert(resp.message);
        this.form.reset();
      } else {
        alert("❌ Admission failed. Please try again.");
      }
    } catch (err) {
      console.error("Error submitting form:", err);
      alert("⚠️ Network or server error.");
    }
  }
}
