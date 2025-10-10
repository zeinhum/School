export function Analytics() {
    const SelectEl = document.querySelector("#slect-target");
    const SelectAct = document.querySelector("#select-action");

    SelectEl.addEventListener("change", (e) => {
        const target = e.target.value;
        SelectAct.innerHTML = ""; // Clear old options

        if (target === "student") {
            let opt1 = document.createElement("option");
            opt1.value = "performance";
            opt1.textContent = "Performance Analysis";

            let opt2 = document.createElement("option");
            opt2.value = "marksheet";
            opt2.textContent = "Print Marksheet";

            SelectAct.appendChild(opt1);
            SelectAct.appendChild(opt2);

        } else {
            let opt = document.createElement("option");
            opt.value = "performance";
            opt.textContent = "Performance Report";
            SelectAct.appendChild(opt);
        }
    });
}
