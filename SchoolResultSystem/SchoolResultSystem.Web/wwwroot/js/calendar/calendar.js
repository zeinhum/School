import { FetchJsonPost, FetchJson } from "../fetchJson.js";
import { PartialNaV } from "../UI/partialNav.js";

export default class CalendarManager {
  constructor() {
    this.map = {
      addRow: this.#addRow.bind(this),
      removeRow: this.#removeRow.bind(this),
      setCallendar: this.#setCallendar.bind(this),
      reset: this.#reset,
    };

    this.nav = new PartialNaV(".partial-container", this.map);
  }

  async #setCallendar() {
    const data = this.#extractData();
    const confirmed = confirm("Add new entires to current year calendar?");
    if (confirmed && data) {
      console.log(data);
      try {
        const res = await FetchJsonPost(
          "/Principal/LeaveCallender/SetCalendar",
          data,
        );
        alert(res.message);
      } catch (error) {
        alert("Error occured");
        console.log(error);
      }
    }
  }

  async #reset() {
    const confirmed = confirm(
      "Callendar for this year will erase all entries, are you sure to do this?",
    );
    if (confirmed) {
      try {
        const res = await FetchJson("/Principal/LeaveCallender/ResetCalendar");
        alert(res.message);
      } catch (error) {
        alert("Callendar could not be reset");
      }
    }
  }
  // HELPERS

  #addRow(e) {
    const weekendRow = e.target.closest("tr");
    const newRow = weekendRow.cloneNode(true);
    const tbody = e.target.closest("tbody");
    tbody.appendChild(newRow);
  }

  #removeRow(e) {
    const row = e.target.closest("tr");
    row.remove();
  }

  #extractData() {
    const Weekdays = [];
    const Leaves = [];

    // Weekend extraction
    document.querySelectorAll(".weekend tbody tr").forEach((row) => {
      const dateInput = row.querySelector('input[type="date"]');
      if (dateInput && dateInput.value) {
        Weekdays.push(dateInput.value);
      }
    });

    // Leave extraction
    document.querySelectorAll(".leave-table tbody tr").forEach((row) => {
      const dateInput = row.querySelector('input[type="date"]');
      const descInput = row.querySelector('input[type="text"]');

      if (dateInput && dateInput.value) {
        Leaves.push({
          Date: dateInput.value,
          Description: descInput.value || "",
        });
      }
    });

    return {
      Weekdays,
      Leaves,
    };
  }

  destroy() {
    this.nav.destroy();
    this.nav = null;
    this.map={};
  }
}
