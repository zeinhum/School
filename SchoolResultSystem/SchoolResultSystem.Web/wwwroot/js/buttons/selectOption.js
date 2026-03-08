// add option
export class SelectOption {
  constructor() {
    this.created = null;
  }
  // add option to selected element
  addOption(value, text, el = this.created) {
    const opt = document.createElement("option");
    opt.value = value;
    opt.textContent = text;
    el.appendChild(opt);
  }

  /// create element
  createEl(targ, el, id = null, clss = null) {
    const elmnt = document.createElement(el);
    if (id) elmnt.id = id;
    if (clss) elmnt.className = clss;
    targ.appendChild(elmnt);
    this.created = elmnt;
    return elmnt;
  }

  // create element
  innerElement(target, tagName, attr = {}) {
    const element = document.createElement(tagName);
    for (const [key, value] of Object.entries(attr)){
      element.setAttribute(key, value);
    }

    target.appendChild(element);
    this.created = element;
    return element;
  }
}

export function catchElement(elm) {
  return document.querySelector(elm);
}

export function listenEvent(elm, event, callback) {
  return elm.addEventListener(event, callback());
}

export class ClickOn {
  constructor() {
    this.el = null;
    this.prvEl = null;
  }

  element(selector, callback) {
    document.addEventListener("click", (e) => {
      this.el = e.target.closest(selector);
      if (!this.el) return;

      if (this.el !== this.prvEl) {
        this.prvEl = this.el;
        callback(e);
      }
    });

    return this; // enable chaining
  }
}
