//import { RedirectButtons } from "./controllModel.js";
import { writeHeader } from "./dashboardHeader.js";
import {Navigation} from "./UI/nevigation.js"



export function RightNav(data){
    // wwrite header from session
        writeHeader(data);
    
    // initialize controll button
//let redirectButtons = new RedirectButtons("/Principal");
const controller ="Principal";
const partialDash =document.querySelector(".partial-container");
new Navigation(controller,partialDash);

}
