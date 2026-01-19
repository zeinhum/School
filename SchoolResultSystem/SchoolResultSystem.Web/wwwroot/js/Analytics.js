export class AnalyticsHandler {
  constructor() {
    // cache important elements
    this.board = document.querySelector(".analysis-board");
    this.idEl = this.board?.querySelector("#id");
    this.selectTarget = this.board?.querySelector("#slect-target");
    this.selectAction = this.board?.querySelector("#select-action");
    this.resultContainer = this.board?.querySelector(".result-container");
    this.subSelection = this.board?.querySelector("#sub-action");
    this.actionButton = this.board?.querySelector(".action");
    this.actionLebel = "";

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
// chose option dynamic
  populateActions(target) {
    this.selectAction.innerHTML = "";
    this.subSelection.innerHTML ="";

    

    if (target === "Student") {
      this.addOption("M", "Select Option", this.selectAction);
      this.addOption("Marksheet", "Marksheet", this.selectAction);
      this.addOption("StudentAttendence", "Attendence Report", this.selectAction);
      this.onChange();
      //this.addOption("Analysis", "Grade Analysis");
    } else if(target=="Teacher"){
      this.addOption("M", "Select Option", this.selectAction);
      this.addOption("TeacherAttendence", "Attendence Report", this.selectAction);
    //  this.addOption("StudentAttendence", "Attendence Report", this.selectAction);
      this.onChange();
    }
    else if (target == "Class") {
      this.addOption("Report", "Exam Report", this.selectAction);
      //this.addOption("Analysis", "Rank");
    } else {
      this.addOption("Performance", "Performance", this.selectAction);
    }
  }


  // add option
  addOption(value, text, el) {
    const opt = document.createElement("option");
    opt.value = value;
    opt.textContent = text;
    el.appendChild(opt);
  }

  // add subotion
  populateSubActions(target){
    const formRow = this.board.querySelector(".form-row");
    const selectElement = document.createElement("select");
    this.subSelection.innerHTML ="";
    this.actionLebel=target;
console.log(`target = ${target}`)
    if(target=="StudentAttendence" || target=="TeacherAttendence"){

      this.subSelection.innerHTML=`<p data-action="/Attendence/Attendence/DisplpayAttendence">From Date:<span>
  <input type="date" id="fromdate" name="date"><span> To Date: <span><input type="date" id="todate" name="date"></span></p>`
  
    } else if(target=="Marksheet"){
      this.subSelection.innerHTML=`<p data-action="/Analytics/Student/Marksheet">Exam Year:<span><input type="text" id="ex-year" name="ex-year"></span>  Exam Name:
      <span><input type="text" id="ex-name"></span></p>`
    }
  }

  /// create element
createEl(targ, el, id = null, clss = null) {
    const elmnt = document.createElement(el);

    if (id) elmnt.id = id;
    if (clss) elmnt.className = clss;

    targ.appendChild(elmnt);
    
    return elmnt; 
}


  // on change in first action
  onChange(){
    const slector =this.selectAction.addEventListener('change',(e)=>{
      this.populateSubActions(e.target.value);

    })
  }

getExamDateName(){
  let exYear = this.subSelection.querySelector("#ex-year");
  exYear = parseInt(exYear.value) ||0;

  let exName = this.subSelection.querySelector("#ex-name");
  exName = exName.value || "default";

  return [exYear,exName]
}


getdates(){
  let fromdate = this.subSelection.querySelector("#fromdate");
  fromdate = fromdate.value;

  let todate = this.subSelection.querySelector("#todate");
  todate = todate.value;

  return [fromdate , todate];
}


  async redirect() {
    const id = this.idEl?.value?.trim();
    const target = this.selectTarget?.value;
    const action = this.selectAction?.value;

    let formBody;
    let targetUrl;
    targetUrl = `/Analytics/${target}/${action}`;

    console.log(`action lebel = ${this.actionLebel}`);

    if (this.actionLebel==="StudentAttendence"){
      const [fromdate , todate] =this.getdates();
      formBody = JSON.stringify({Id:id, CandidateType:"student", CandidateName:"x", From:fromdate, Till:todate});
      
      targetUrl=this.subSelection.querySelector('p').dataset.action;
    }else if (this.actionLebel==="TeacherAttendence") {
      const [fromdate , todate] =this.getdates();
      formBody = JSON.stringify({Id:id, CandidateType:"teacher", CandidateName:"x", From:fromdate, Till:todate});
      
      targetUrl=this.subSelection.querySelector('p').dataset.action;

    } else if(this.actionLebel==="Marksheet"){
      // get exam date and name
      const [exYear, exName]=this.getExamDateName();
      formBody = JSON.stringify({NSN:id, exYear:exYear, exName:exName});
      targetUrl=this.subSelection.querySelector('p').dataset.action;
    }
    // subction
    const subaction =this.subSelection?.value;

    if (!id || !target || !action) {
      alert("Please fill all fields before continuing.");
      return;
    }

    
    console.log("Fetching:", targetUrl);
    console.log("id:", JSON.stringify(id));

    try {
      const response = await fetch(targetUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: formBody,
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
