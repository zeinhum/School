import { TableRowDuplicate, RowDataCollector } from "./FormModules.js";
import { PartialNaV } from "./UI/partialNav.js";
import { FetchJsonPost } from "./fetchJson.js";




export default class ClassSubject {
  constructor() {
    this.rowDuplicate = new TableRowDuplicate();
    this.dataCollect = null;
    this.fetchJson = null;
    this.map = {
      submit: this.#submit,
      subselected: this.subSelect
    };
    this.nav = new PartialNaV(".partial-container",this.map);
  }
  async #submit() {
    if (!this.dataCollect) {
      this.dataCollect = new RowDataCollector(".form-data");
    }
    const data = this.dataCollect.getData();
    if (data) {
      const res = await FetchJsonPost("/Principal/SetupPrincipal/SaveSubject",data);
      alert(res.message);
    }
  }

  subSelect(e) {
    console.log("sebSelect called")
    const type = e.target.value;
    console.log(type);
    let parentrow = e.target.closest("tr");
    let td = parentrow.querySelector(".pr");

    if (type === "theory") {
      td.innerHTML = `<input type="text" name="LinkedPr" required size="2" placeholder="CODE"/>`;
    } else if (type === "practical") {
      td.innerHTML = "None";
    }
  }

  destroy() {
    this.rowDuplicate = null;
    this.nav.destroy();
    this.nav = null;
    this.dataCollect = null;
    this.fetchJson = null;
  }
}

