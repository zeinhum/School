
// post method
export async function FetchJsonPost(microservice, payload) {
    try {
        const response = await fetch(microservice, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(payload)
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        //console.log(data); // actual JSON object
        return data;
    } catch (error) {
        console.error("Error fetching JSON:", error);
        return null;
    }
}
export function FetchJson(){

}

export async function SendData(microservice, data) {
    //console.log(`data= ${JSON.stringify(data)}`)
    try {
        const res = await fetch(microservice, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        });

        if (!res.ok) {
            console.error("HTTP error:", res.status);
            return false;
        }

        const responseData = await res.json();
        //console.log(responseData)

        // If your API returns { success: true/false }
        return responseData;
    } catch (error) {
        console.error("Fetch error:", error);
        return false;
    }
}

