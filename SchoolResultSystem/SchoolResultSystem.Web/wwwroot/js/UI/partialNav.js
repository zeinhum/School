export class PartialNaV {
  constructor(rootSelector = ".partial-container", map) {
    this.root = document.querySelector(rootSelector);
    this.funMap = map

    if (!this.root) {
      console.warn(`root element "${rootSelector}" not found.`);
      return;
    }

    this.handleClick = this.handleClick.bind(this); 
    this.handleChange = this.handleChange.bind(this); 

    this.init();
  }


  init() {
    this.root.addEventListener("click", this.handleClick);
    this.root.addEventListener("change", this.handleChange);
  }

 

  async handleClick(e) {
    try {
      const actionEl = e.target.closest("[data-action]");
      if (!actionEl || !this.root.contains(actionEl)) return;
      const { action, id } = actionEl.dataset;
      //console.log(action, id)
      //console.log(this.funMap[action]);
      const handler = this.funMap[action];
      if (!action || !handler) return;
      if(!id) return await handler.call(this,e);
      await handler.call(this, id, e);
    } catch (err) {
      console.error("handleClick error:", err);
      alert("Something went wrong. Please try again.");
    }
  }

  async handleChange(e) {
    try {
      const actionEl = e.target.closest("[data-change]");
      if (!actionEl || !this.root.contains(actionEl)) return;
      const { change, id } = actionEl.dataset;
     // console.log(change, id)
      //console.log(this.funMap[action]);
      const handler = this.funMap[change];
      if (!change || !handler) return;
      if(!id) return await handler.call(this, e);
      await handler.call(this, id, e);
    } catch (err) {
      console.error("handleChange error:", err);
      alert("Something went wrong. Please try again.");
    }
  }

// cleanup

  destroy() {
    if (this.root) {
      this.root.removeEventListener("click", this.handleClick);
      this.root.removeEventListener("change", this.handleChange);
    }

    this.root = null;
    this.funcMap = null;
    this.handleClick = null;
    this.handleChange = null;
    //console.log("cache cleaned")
  }
}