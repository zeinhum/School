import { TableRowDuplicate, RowDataCollector } from "./FormModules.js";
import { PartialNaV } from "./UI/partialNav.js";
import { FetchJsonPost } from "./fetchJson.js";

export default class Classes {
  constructor() {
    this.rowDuplicate = new TableRowDuplicate();
    this.formSubmit = null;
    this.dataCollector =null;
    this.map = {
      submit: this.#submit,
    };

    this.nav = new PartialNaV(".partial-container", this.map);
  }

  async #submit() {
    if(!this.dataCollector){
      this.dataCollector=new RowDataCollector(".form-data");
    }
    
    const data = this.dataCollector.getData();
    console.log(data);
    if (data) {
      const res = await FetchJsonPost("/Principal/SetupPrincipal/SaveClass", data);
      alert(res.message)
      return;
    }
    alert("no data found");
  }

  destroy() {
    this.nav.destroy();
    this.rowDuplicate = null;
    this.formSubmit = null;
    this.nav = null;
  }
}
