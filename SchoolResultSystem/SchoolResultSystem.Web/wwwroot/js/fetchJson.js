export function FetchJson(microservice) {
    return fetch(microservice)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json(); // parse JSON here
        })
        .then(data => {
            console.log(data); // actual JSON object
            return data;       // return parsed JSON
        })
        .catch(error => {
            console.error("Error fetching JSON:", error);
            return null;
        });
}


export async function SendData(microservice, data) {
    console.log(`data= ${JSON.stringify(data)}`)
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
        console.log(responseData)

        // If your API returns { success: true/false }
        return responseData;
    } catch (error) {
        console.error("Fetch error:", error);
        return false;
    }
}

