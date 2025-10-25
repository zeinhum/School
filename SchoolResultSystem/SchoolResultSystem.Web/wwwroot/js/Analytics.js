export class AnalyticsHandler {
  constructor() {
    // cache important elements
    this.board = document.querySelector(".analysis-board");
    this.idEl = this.board?.querySelector("#id");
    this.selectTarget = this.board?.querySelector("#slect-target");
    this.selectAction = this.board?.querySelector("#select-action");
    this.resultContainer = this.board?.querySelector(".result-container");
    this.actionButton = this.board?.querySelector(".action");

    this.init();
  }

  init() {
    if (!this.selectTarget || !this.selectAction || !this.actionButton) {
      console.warn("Analytics UI elements not found in DOM.");
      return;
    }

    // Populate actions dynamically
    this.selectTarget.addEventListener("change", (e) => {
      this.populateActions(e.target.value);
    });

    // Handle click
    this.actionButton.addEventListener("click", () => this.redirect());
  }

  populateActions(target) {
    this.selectAction.innerHTML = "";

    if (target === "Student") {
      this.addOption("Marksheet", "Recent Marksheet");
      this.addOption("Analysis", "Grade Analysis");
    } else if (target == "Class") {
      this.addOption("Report", "Exam Report");
      this.addOption("Analysis", "Rank");
    } else {
      this.addOption("Performance", "Performance");
    }
  }

  addOption(value, text) {
    const opt = document.createElement("option");
    opt.value = value;
    opt.textContent = text;
    this.selectAction.appendChild(opt);
  }

  async redirect() {
    const id = this.idEl?.value?.trim();
    const target = this.selectTarget?.value;
    const action = this.selectAction?.value;

    if (!id || !target || !action) {
      alert("Please fill all fields before continuing.");
      return;
    }

    const targetUrl = `/Analytics/${target}/${action}`;
    console.log("Fetching:", targetUrl);
    console.log("id:", JSON.stringify(id));

    try {
      const response = await fetch(targetUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ NSN: id }),
      });

      const bodyText = await response.text(); // ✅ read once

      if (!response.ok) {
        this.resultContainer.innerHTML = `<div style=color:black>bodyText</div>`;
        throw new Error(bodyText || "Request failed");
      }

      this.resultContainer.innerHTML = bodyText;
      this.Printer();
    } catch (err) {
      console.error("Error fetching analytics data:", err);
      this.resultContainer.innerHTML = `<div style=color:black>Oops! ${err}</div>`;
    }
  }

  Printer() {
    document.querySelector(".print")?.addEventListener("click", (e) => {
      e.preventDefault();
      window.print();
    });
  }
}
