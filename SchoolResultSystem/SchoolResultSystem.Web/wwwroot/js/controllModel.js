import { AnalyticsHandler } from "./Analytics.js";
import { MoveStudents } from "./moveStudents.js";

export class RedirectButtons {
  constructor(baseDir, btnClass = ".btn") {
    this.baseDir = baseDir;
    this.btnClass = btnClass;
    this.toggle = document.querySelector(".right-nav");
    this.container = document.querySelector(".partial-container");



    this.jsContainer = null;
    this.#initListener();
    this.#toggler();
  }

  // --- Private: Event listener for general redirect buttons ---
  #initListener() {
    document.body.addEventListener("click", (e) => {
      const button = e.target.closest(this.btnClass);
      if (!button) return;

      e.preventDefault();

      const action = button.dataset.action;
      const id = button.dataset.id || "";

      if (action) {
        this.#redirect(action, id);
        return;
      }
      const move = button.dataset.move;
      if (move) {
        const id = button.dataset.id
        MoveStudents(id); // students to next class
      }
    });
  }

  //-- action on change--

  async actionOnChange(selectionid, controller) {
  const selector = document.querySelector(`#${selectionid}`);
  if (!selector) return;

  selector.addEventListener("change", async (e) => {
    e.preventDefault();

   const selected = e.target.options[e.target.selectedIndex];

  const id = selected.value;
  const action = selected.dataset.action;

    console.log(`action on changer url : ${controller}/${action}/${id}`);

    // Wait for the server partial view
    await this.#fetchPartial(controller, action, id);
  });
}


  // --- Private: Navigation toggler (load partials dynamically) ---
  async #toggler(base = "PrincipalDashboard") {

    if (!this.toggle) {
      console.warn("No element found for navigation");
      return;
    }

    this.toggle.addEventListener("click", async (e) => {
      const button = e.target.closest(".toggler");
      if (!button) return;

      const action = button.dataset.action;

      try {
        switch (action) {
          case "Analytics":
            await this.#fetchPartial(base, action);
            this.jsContainer = new AnalyticsHandler();
            break;
          case "Teachers":
            await this.#fetchPartial(base, action);
            this.jsContainer = null;
            break;

          case "Classes":
            await this.#fetchPartial(base, action);
            this.jsContainer = null;
            break;

          default:
            console.warn(`Unknown action: ${action}`);
        }
      } catch (err) {
        console.error(`Error loading ${action}:`, err);
      }
    });
  }

  // --- Private: Page redirect ---
  #redirect(action, id = "") {
    const url = `${this.baseDir}/${action}${id ? `?id=${id}` : ""}`;
    window.location.href = url;
  }

  // --- Private: Partial fetch loader ---
  async #fetchPartial(base, action, id = "") {
    const url = `${this.baseDir}/${base}/${action}${id ? `?id=${id}` : ""}`;

    if (!this.container) {
      console.error("Partial container not found.");
      return;
    }

    try {
      this.container.innerHTML = `<p class="loading">Loading...</p>`;

      const response = await fetch(url);
      if (!response.ok)
        throw new Error(`HTTP error! status: ${response.status}`);

      const htmlText = await response.text();
      this.container.innerHTML = htmlText;
    } catch (error) {
      console.error("Failed to fetch partial:", error);
      this.container.innerHTML = `<p class="error-message">Failed to load content.</p>`;
    }
  }


  // For API calls and receive promise
  // baseUrl: ARea/Controller
  // action: api endpoint 
  // Home/Login/authenticate
  // json data
  async #CallApi(baseUrl,action, data){
    const api =baseUrl/action;
      //console.log(`data being sent: ${JSON.stringify(data)}`);
    try{
      const response = await fetch(api, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      })
      return response;
  } catch{
    return "no promise";
  }
    }
      
}
