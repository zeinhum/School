import { Connection } from "../connection/connection.js";

export class Navigation {
  constructor(controller,dasboard) {
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
      let partialUrl = e.target.closest(".partial"); // laoding partial contents
      let redirectUrl = e.target.closest(".redirect");
      const url = e.target.dataset.to;
      const id = e.target.dataset.id;
      //console.log(url);
      if (partialUrl) {
        const jsUrl = e.target.dataset.js;
        console.log(jsUrl)
        const htmltext = await this.connection.getHTML(`${this.controller}/${url}`,id);
        this.dashboard.innerHTML =htmltext;
        await this.connection.importjs(jsUrl);
      } else if (redirectUrl) {
        
        this.connection.redirect(`${this.controller}/${url}`,id)
      }
    } catch (Exception) {
      console.log(Exception)
      alert("something misbehaved at navigation.");
    }
  }
}
