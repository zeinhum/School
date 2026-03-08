import { SelectOption } from "./buttons/selectOption.js";



export default class AnalyticsHandler {
  constructor() {
    // Cache important elements
    this.board = document.querySelector(".card");
    this.idEl = this.board?.querySelector("#id");
    this.selectTarget = this.board?.querySelector("#slect-target");
    this.selectAction = this.board?.querySelector("#select-action");
    this.resultContainer = document.querySelector(".result-container");
    this.subSelection = this.board?.querySelector("#sub-action");
    this.actionButton = this.board?.querySelector(".action");
    this.actionLabel = "";
    this.selectOpt = new SelectOption();
    
    // 🔥 NEW: Track all event listeners for cleanup
    this.boundHandlers = new Map();
    this.abortController = null; // For fetch cancellation
    this.init();
  }

  init() {
    
    if (!this.selectTarget || !this.selectAction || !this.actionButton) {
      console.warn("Analytics UI elements not found in DOM.");
      return;
    }

    // Populate actions dynamically
    const targetChangeHandler = (e) => this.populateActions(e.target.value);
    this.selectTarget.addEventListener("change", targetChangeHandler);
    this.boundHandlers.set(this.selectTarget, { 
      event: "change", 
      handler: targetChangeHandler 
    });

    // Handle click
    const actionClickHandler = () => this.redirect();
    this.actionButton.addEventListener("click", actionClickHandler);
    this.boundHandlers.set(this.actionButton, { 
      event: "click", 
      handler: actionClickHandler 
    });

    //console.log("AnalyticsHandler initialized");
  }

  // Choose option dynamic
  populateActions(target) {
    this.selectAction.innerHTML = "";
    this.subSelection.innerHTML = "";

    if (target === "Student") {
      this.selectOpt.addOption("M", "Select Option", this.selectAction);
      this.selectOpt.addOption("Marksheet", "Marksheet", this.selectAction);
      this.selectOpt.addOption("StudentAttendence", "Attendence Report", this.selectAction);
      this.onChange();
    } else if (target === "Teacher") {
      this.selectOpt.addOption("M", "Select Option", this.selectAction);
      this.selectOpt.addOption("TeacherAttendence", "Attendence Report", this.selectAction);
      this.onChange();
    } else if (target === "SchoolClass") {
      this.selectOpt.addOption("M", "Select Option", this.selectAction);
      this.selectOpt.addOption("ExamReport", "Exam Report", this.selectAction);
    } else {
      this.selectOpt.addOption("Performance", "Performance", this.selectAction);
    }
  }

  // Add sub-option
  populateSubActions(target) {
    this.subSelection.innerHTML = "";
    this.actionLabel = target;

    if (target === "StudentAttendence" || target === "TeacherAttendence") {
      this.subSelection.innerHTML = `<p data-action="/Attendence/Attendence/DisplpayAttendence">
        From Date:<span><input type="date" id="fromdate" name="date"></span> 
        To Date: <span><input type="date" id="todate" name="date"></span></p>`;
    } else if (target === "Marksheet") {
      this.subSelection.innerHTML = `<p data-action="/Analytics/Student/${target}">
        Exam Year:<span><input type="text" id="ex-year" name="ex-year"></span>  
        Exam Name:<span><input type="text" id="ex-name"></span></p>`;
    } else if (target === "ExamReport") {
      this.subSelection.innerHTML = `<p data-action="/Analytics/Class/${target}">
        Exam Year:<span><input type="text" id="ex-year" name="ex-year"></span>  
        Exam Name:<span><input type="text" id="ex-name"></span></p>`;
    }
  }

  // On change in first action
  onChange() {
    // 🔥 FIXED: Remove previous listener if exists
    const existingHandler = this.boundHandlers.get(this.selectAction);
    if (existingHandler && existingHandler.event === "change") {
      this.selectAction.removeEventListener(
        existingHandler.event, 
        existingHandler.handler
      );
    }

    // Add new listener
    const actionChangeHandler = (e) => this.populateSubActions(e.target.value);
    this.selectAction.addEventListener("change", actionChangeHandler);
    this.boundHandlers.set(this.selectAction, { 
      event: "change", 
      handler: actionChangeHandler 
    });
  }

  getExamDateName() {
    let exYear = this.subSelection.querySelector("#ex-year");
    exYear = parseInt(exYear.value) || 0;

    let exName = this.subSelection.querySelector("#ex-name");
    exName = exName.value || "default";

    return [exYear, exName];
  }

  getdates() {
    let fromdate = this.subSelection.querySelector("#fromdate");
    fromdate = fromdate.value;

    let todate = this.subSelection.querySelector("#todate");
    todate = todate.value;

    return [fromdate, todate];
  }

  async redirect() {
    const id = this.idEl?.value?.trim();
    const target = this.selectTarget?.value;
    const action = this.selectAction?.value;

    let formBody;
    let targetUrl;
    targetUrl = `/Analytics/${target}/${action}`;

    if (this.actionLabel === "StudentAttendence") {
      const [fromdate, todate] = this.getdates();
      formBody = JSON.stringify({
        Id: id,
        CandidateType: "student",
        CandidateName: "x",
        From: fromdate,
        Till: todate
      });
      targetUrl = this.subSelection.querySelector("p").dataset.action;
    } else if (this.actionLabel === "TeacherAttendence") {
      const [fromdate, todate] = this.getdates();
      formBody = JSON.stringify({
        Id: id,
        CandidateType: "teacher",
        CandidateName: "x",
        From: fromdate,
        Till: todate
      });
      targetUrl = this.subSelection.querySelector("p").dataset.action;
    } else if (this.actionLabel === "Marksheet" || this.actionLabel === "ExamReport") {
      const [exYear, exName] = this.getExamDateName();
      formBody = JSON.stringify({ NSN: id, exYear: exYear, exName: exName });
      targetUrl = this.subSelection.querySelector("p").dataset.action;
    }

    if (!id || !target || !action) {
      alert("Please fill all fields before continuing.");
      return;
    }

    // 🔥 NEW: Create abort controller for this fetch
    this.abortController = new AbortController();

    try {
      const response = await fetch(targetUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: formBody,
        signal: this.abortController.signal // ✅ Allow cancellation
      });

      const bodyText = await response.text();

      if (!response.ok) {
        throw new Error(bodyText || "Request failed");
      }

      this.resultContainer.innerHTML = bodyText;
      this.Printer();
    } catch (err) {
      // Don't show error if fetch was aborted (user navigated away)
      if (err.name === "AbortError") {
        console.log("Fetch aborted");
        return;
      }
      
      console.error("Error fetching analytics data:", err);
      this.resultContainer.innerHTML = `<div style="color:red">Oops! ${err}</div>`;
    }
  }

  Printer() {
    const printBtn = document.querySelector(".print");
    
    if (!printBtn) return;

    // 🔥 FIXED: Remove previous print handler if exists
    const existingPrintHandler = this.boundHandlers.get(printBtn);
    if (existingPrintHandler) {
      printBtn.removeEventListener(
        existingPrintHandler.event,
        existingPrintHandler.handler
      );
    }

    // Add new print handler
    const printHandler = (e) => {
      e.preventDefault();
      window.print();
    };
    
    printBtn.addEventListener("click", printHandler);
    this.boundHandlers.set(printBtn, { 
      event: "click", 
      handler: printHandler 
    });
  }

  // 🔥 NEW: Cleanup method - CRITICAL!
  destroy() {
    //console.log("AnalyticsHandler cleanup started");

    // 1. Remove all event listeners
    this.boundHandlers.forEach((config, element) => {
      if (element) {
        element.removeEventListener(config.event, config.handler);
      }
    });
    this.boundHandlers.clear();

    // 2. Abort any pending fetch requests
    if (this.abortController) {
      this.abortController.abort();
      this.abortController = null;
    }

    // 3. Clear innerHTML to remove dynamically added content
    if (this.resultContainer) {
      this.resultContainer.innerHTML = "";
    }
    if (this.selectAction) {
      this.selectAction.innerHTML = "";
    }
    if (this.subSelection) {
      this.subSelection.innerHTML = "";
    }

    // 4. Nullify all DOM references
    this.board = null;
    this.idEl = null;
    this.selectTarget = null;
    this.selectAction = null;
    this.resultContainer = null;
    this.subSelection = null;
    this.actionButton = null;
    this.actionLabel = "";

    //console.log("AnalyticsHandler cleanup complete");
  }
}