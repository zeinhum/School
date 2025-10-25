export class TableRowDuplicate {
  constructor(tbodySelector = ".form-data") {
    this.tbody = document.querySelector(tbodySelector);
    this.tbodySelector = tbodySelector;

    const firstRow = this.tbody.querySelector("tr");
    if (firstRow) this.addControlBtns(firstRow);
  }

  replicateRow() {
    const row = this.tbody.querySelector("tr"); // template row
    const newRow = row.cloneNode(true);

    newRow.querySelectorAll("input").forEach((i) => (i.value = ""));
    newRow.querySelectorAll("select").forEach((s) => (s.selectedIndex = 0));

    this.addControlBtns(newRow);
    this.tbody.appendChild(newRow);
  }

  removeRow(row) {
    if (this.tbody.rows.length > 1) row.remove();
  }

  addControlBtns(row) {
    const existing = row.querySelector(".btn-group");
    if (existing) existing.remove();

    const td = document.createElement("td");
    td.className = "btn-group";

    const removeBtn = document.createElement("button");
    removeBtn.type = "button";
    removeBtn.textContent = "-";
    removeBtn.className = "btn-remove";
    removeBtn.onclick = () => this.removeRow(row);

    const addBtn = document.createElement("button");
    addBtn.type = "button";
    addBtn.textContent = "+";
    addBtn.className = "btn-add";
    addBtn.onclick = () => this.replicateRow();

    td.append(removeBtn, addBtn);
    row.appendChild(td);
  }
}

export class RowDataCollector {
  constructor(tbodySelector = ".form-data", errorSelector = ".error") {
    this.tbodySelector = tbodySelector;
    this.errorBox = document.querySelector(errorSelector);
  }

  getData() {
    const rows = document.querySelectorAll(`${this.tbodySelector} tr`);
    let formData = [];
    let hasError = false;
    this.errorBox.innerHTML = "";

    for (const [index, row] of rows.entries()) {
      const fields = row.querySelectorAll("input, select, textarea");
      const rowData = {};

      for (const field of fields) {
        const name = field.hasAttribute("name")
          ? field.getAttribute("name").trim()
          : null;

        const value = field.value?.trim();

        if (!value) {
          this.errorBox.innerHTML = `Some fields are empty in row ${
            index + 1
          }. Fill the form correctly.`;
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
