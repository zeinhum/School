import { RedirectButtons } from "./controllModel.js";
import { writeHeader } from "./dashboardHeader.js";



export function RightNav(data){
    // wwrite header from session
        writeHeader(data);
    
    // initialize controll button
let redirectButtons = new RedirectButtons("/Principal");

}
