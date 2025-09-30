export class RedirectButtons {
  constructor(baseDir, btnClass = ".btn") {
    this.baseDir = baseDir;
    this.btnClass = btnClass;
    this.#initListener();
  }

  // --- Private: Event listener ---
  #initListener() {
    document.body.addEventListener("click", (e) => {
      const button = e.target.closest(this.btnClass);
      if (!button) return;

      e.preventDefault();

      const action = button.dataset.action;   // e.g. "DeleteTeacher"
      const id = button.dataset.id || "";     // optional ID

      if (!action) {
        console.warn("No data-action found on button:", button);
        return;
      }

      this.#redirect(action, id);
    });
  }

  // --- Private: Fetch wrapper ---
  #redirect(action, id = "") {
    const url = `${this.baseDir}/${action}${id ? `?id=${id}` : ""}`;
    window.location.href=url;
  }
}
