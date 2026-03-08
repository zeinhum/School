import { Connection } from "../connection/connection.js";

export class Navigation {
  constructor(controller, dasboard) {
    this.dashboard = dasboard;
    this.controller = controller;
    this.connection = new Connection();
    this.partialScript = null;
    this.#eventDeligation();
  }

  #eventDeligation() {
    document.body.addEventListener("click", async (e) => {
      await this.#handleclick(e);
    });
  }

  // click handler
  async #handleclick(e) {
    try {
      const target = e.target;
      const url = target.dataset.to;
      const id = target.dataset.id;

      if (target.classList.contains("partial")) {
        const jsUrl = target.dataset.js;
        const htmltext = await this.connection.getHTML(`${this.controller}/${url}`, id);
        this.dashboard.innerHTML = htmltext;
        await this.connection.importjs(jsUrl);
      } else if (target.classList.contains("redirect")) {
        this.connection.redirect(`${this.controller}/${url}`, id);
      } else if (target.classList.contains("external")) {
        window.open(url, "_blank");
      }
    } catch (Exception) {
      console.log(Exception);
      alert("something misbehaved at navigation.");
    }
  }
}