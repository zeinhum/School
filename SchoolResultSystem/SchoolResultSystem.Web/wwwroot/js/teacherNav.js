import { RedirectButtons } from "./controllModel.js";
import { writeHeader } from "./dashboardHeader.js";


export function TNav(data){
    // wwrite header from session
        writeHeader(data);
    
    // initialize controll button
let redirectButtons = new RedirectButtons("/Teacher");
}