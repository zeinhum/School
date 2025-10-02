// wwwroot/js/dynamic-class-input.js
export class DynamicClassInput {
    constructor(formSelector, inputName = "ClassName") {
        this.form = document.querySelector(formSelector);
        if (!this.form) {
            console.error(`Form not found: ${formSelector}`);
            return;
        }
        this.inputName = inputName;

        const originalInput = this.form.querySelector(`input[name='${this.inputName}']`);
        const parentGroup = originalInput.parentElement;

        this.wrapInput(parentGroup, originalInput);
        this.updateAllControls();

        this.form.addEventListener("submit", () => {
            this.form.action = this.form.getAttribute("action");
        });
    }

    wrapInput(parent, inputEl) {
        const wrapper = document.createElement("div");
        wrapper.className = "class-input-row";
        parent.replaceChild(wrapper, inputEl);
        wrapper.appendChild(inputEl);

        this.addControls(wrapper, inputEl);
        inputEl.addEventListener("input", () => this.updateControls(wrapper, inputEl));
    }

    addControls(wrapper, inputEl) {
        const btnGroup = document.createElement("div");
        btnGroup.className = "btn-group";
        wrapper.appendChild(btnGroup);

        const addBtn = document.createElement("button");
        addBtn.type = "button";
        addBtn.textContent = "+";
        addBtn.className = "btn-add";
        addBtn.onclick = () => this.addNewRow(wrapper);
        btnGroup.appendChild(addBtn);

        const removeBtn = document.createElement("button");
        removeBtn.type = "button";
        removeBtn.textContent = "-";
        removeBtn.className = "btn-remove";
        removeBtn.onclick = () => this.removeRow(wrapper);
        btnGroup.appendChild(removeBtn);
    }

    updateControls(wrapper, inputEl) {
        const addBtn = wrapper.querySelector(".btn-add");
        const removeBtn = wrapper.querySelector(".btn-remove");
        const allRows = this.form.querySelectorAll(".class-input-row");

        if (inputEl.value.trim() && allRows.length > 1) {
            removeBtn.style.display = "inline-block";
        } else if (inputEl.value.trim() && allRows.length === 1) {
            removeBtn.style.display = "inline-block";
        } else {
            removeBtn.style.display = "none";
        }

        addBtn.style.display =
            (allRows[allRows.length - 1] === wrapper) ? "inline-block" : "none";
    }

    addNewRow(afterWrapper) {
        const newWrapper = document.createElement("div");
        newWrapper.className = "class-input-row";

        const newInput = document.createElement("input");
        newInput.type = "text";
        newInput.name = this.inputName;
        newInput.placeholder = "e.g. Class 9 A or Grade 7";
        newWrapper.appendChild(newInput);

        this.addControls(newWrapper, newInput);
        afterWrapper.parentNode.insertBefore(newWrapper, afterWrapper.nextSibling);

        newInput.addEventListener("input", () => this.updateControls(newWrapper, newInput));
        this.updateAllControls();
    }

    removeRow(wrapper) {
        const allRows = this.form.querySelectorAll(".class-input-row");
        if (allRows.length === 1) {
            const input = wrapper.querySelector("input");
            input.value = "";
            this.updateControls(wrapper, input);
            return;
        }
        wrapper.remove();
        this.updateAllControls();
    }

    updateAllControls() {
        const allRows = [...this.form.querySelectorAll(".class-input-row")];
        allRows.forEach(row => {
            const inputEl = row.querySelector("input");
            this.updateControls(row, inputEl);
        });
    }
}
