export class PartialNaV {
  constructor(rootSelector = ".partial-container", map) {
    this.root = document.querySelector(rootSelector);
    this.funMap = map

    if (!this.root) {
      console.warn(`MoveTeachers: root element "${rootSelector}" not found.`);
      return;
    }

    this.handleClick = this.handleClick.bind(this); 

    this.init();
  }


  init() {
    this.root.addEventListener("click", this.handleClick);
  }

 

  async handleClick(e) {
    try {
      const actionEl = e.target.closest("[data-action]");
      if (!actionEl || !this.root.contains(actionEl)) return;
      const { action, id } = actionEl.dataset;
      console.log(action, id)
      console.log(this.funMap[action]);
      const handler = this.funMap[action];
      if (!action || !handler) return;
      if(!id) return await handler.call(this);
      await handler.call(this, id);
    } catch (err) {
      console.error("MoveTeachers handleClick error:", err);
      alert("Something went wrong. Please try again.");
    }
  }

// cleanup

  destroy() {
    if (this.root) {
      this.root.removeEventListener("click", this.handleClick);
    }

    this.root = null;
    this.funcMap = null;
    this.handleClick = null;
    console.log("cache cleaned")
  }
}