export class Connection {
  constructor() {
    this.jsContainer = null;
  }
  /*
  async #fetchurl(url, id) {
    try {
      const response = await fetch(url);

      if (!response.ok) {
        throw new Error(`HTTP error: ${response.status}`);
      }

      return response;
    } catch (error) {
      console.error("Fetch failed:", error);
      alert("Network or server error occurred.");
      return null;
    }
  }
*/
  redirect(url, id) {
    window.location.href = `/${url}/${id ? `?id=${id}` : ""}`;

    return;
  }

  async getHTML(url, id) {
    try {
      const text = await fetch(`/${url}${id ? `?id=${id}` : ""}`).then(
        (res) => res.text(),
      );
      if (text) return text;
    } catch (Exception) {
      return "No content loaded.";
    }
  }
/*
  async fetchJson(url, payload) {
    console.log("fetch json called");
    if (!payload) {
      const jsonstring = await this.#fetchurl(url).then((res) => res.json());
      if (jsonstring) return jsonstring;
      return null;
    }
    console.log("url", url);
    const response = await fetch(url, {
      method: "post",
      headers: {
        "Content-type": "application/json",
      },
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      const data = response.json();
      console.log(data);
      return data;
    } else {
      alert("something misbehaved.");
      return null;
    }
  }
*/
  async importjs(url = undefined) {
    if (this.jsContainer) {
      if (typeof this.jsContainer.destroy === "function") {
        this.jsContainer.destroy();
      }
      this.jsContainer = null;
    }

    if (!url) return;

    try {
      const module = await import(`${url}.js`);

      // Wait for the browser to finish the DOM update from innerHTML
      return new Promise((resolve) => {
        window.requestAnimationFrame(() => {
          this.jsContainer = new module.default();
          resolve(this.jsContainer);
        });
      });
    } catch (exception) {
      console.error("Error at partial JS component:", exception);
      alert("Error loading component logic.");
    }
  }
}
