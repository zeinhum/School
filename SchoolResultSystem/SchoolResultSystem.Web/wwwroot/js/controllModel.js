//import { AnalyticsHandler } from "./Analytics.js";
import { MoveStudents, deactivateStudent } from "./moveStudents.js";
import { changePassword, deactivateTeacher, removeCST } from "./moveTeacher.js";

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

      const id = button.dataset.id || "";
      const action = button.dataset.action;
      const move = button.dataset.move;
      const change = button.dataset.change;

      // 1️⃣ Handle general redirect actions
      if (action) {
        this.#redirect(action, id);
        return;
      }

      // 2️⃣ Move students to other class
      if (move) {
        MoveStudents(id); // implement API / logic inside MoveStudents
        return;
      }

      // 3️⃣ Handle destructive / sensitive changes
      if (change) {
        switch (change) {
          case "deactivateTeacher":
            this.#confirmAndExecute(
              `Are you sure you want to deactivate teacher ${id}?`,
              () => deactivateTeacher(id),
            );
            break;

          case "deactivateStudent":
            this.#confirmAndExecute(
              `Are you sure you want to deactivate student ${id}?`,
              () => deactivateStudent(id),
            );
            break;

          case "changePassword":
            changePassword(id);
            break;


          case "RCST":
            this.#confirmAndExecute("Are you sure to remove this teacher from class-subject?",
              ()=>removeCST(button)
            );
            
            break;

          default:
            console.warn("Unknown change action:", change, "ID:", id);
        }
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
      if (selected) {
        const id = selected.value;
        const action = selected.dataset.action;

        //console.log(`action on changer url : ${controller}/${action}/${id}`);

        // Wait for the server partial view
        await this.#fetchPartial(controller, action, id);
      }
    });
  }

  // --- Private: Navigation toggler (load partials dynamically) ---
  async #toggler(base = this.baseDir + "Dashboard") {
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
            this.jsContainer = await import("./Analytics.js");
            new this.jsContainer.AnalyticsHandler().init();
            break;
          case "Teachers":
            await this.#fetchPartial(base, action);
            this.jsContainer = null;
            break;

          case "Classes":
            await this.#fetchPartial(base, action);
            this.jsContainer = null;
            break;
          
          case "Subjects":
            await this.#fetchPartial(base, action);
            this.jsContainer=null;
            break;

          case "Exams":
            await this.#fetchPartial(base, action)
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
    const url = `${this.baseDir}${base}/${action}${id ? `?id=${id}` : ""}`;
    console.log(`url in old model ${url}`)

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

  // ---------------------------
  // Helper: confirm then execute
  // ---------------------------
  #confirmAndExecute(message, callback) {
    try {
      const confirmed = window.confirm(message);
      if (confirmed && typeof callback === "function") {
        callback();
      }
    } catch (e) {
      console.error("Confirmation error:", e);
    }
  }
  // For API calls and receive promise
  // baseUrl: ARea/Controller
  // action: api endpoint
  // Home/Login/authenticate
  async PostCallApi(ApiUrl, data) {
    const api = baseUrl / action;
    //console.log(`data being sent: ${JSON.stringify(data)}`);
    try {
      const response = await fetch(ApiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      return response;
    } catch {
      return "no promise";
    }
  }
}
