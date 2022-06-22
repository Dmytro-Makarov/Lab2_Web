const uri = 'api/Countries';                      
let countries = [];                                        

function getCountries() { 
    fetch(uri)                                           
        .then(response => {
            if(!response.ok) {                            
                return response.text().then(text => {
                    throw new Error(text) 
                })}                                        
            
            document.getElementById('errorDB').innerHTML = "";
            return response.json();})                      
    .then(data => _displayCountries(data))                   
    .catch(error => document.getElementById('errorDB')
     .innerHTML = error.toString()); } 
function addCountry() {
     const addNameTextbox = document.getElementById('add-Name');
     const addLeaderNameTextbox = document.getElementById('add-LeaderName');
     const addBlockTextbox = document.getElementById('add-Block');
     const addMilMightTextbox = document.getElementById('add-MilMight');
     const addContinentTextbox = document.getElementById('add-Continent');
     
    const country = { 
        countryName: addNameTextbox.value.trim(), 
        leaderName: addLeaderNameTextbox.value.trim(), 
        block: addBlockTextbox.value.trim(), 
        milMight: addMilMightTextbox.value.trim(),
        continentsIds: addContinentTextbox.value.replaceAll(" ",
            "").split(",") }; 
    // метод POST 
     fetch(uri, {
         method: 'POST',
         headers: { 'Accept': 'application/json',
             'Content-Type': 'application/json' 
         },
         body: JSON.stringify(country)
     })
         .then(response => {
             if(!response.ok){        
         return response.text().then(text => {
             throw new Error(text) 
         })}                           
          document.getElementById('errorDB').innerHTML = "";
             return response.json();})
     .then(() => {
         getCountries(); 
         addNameTextbox.value = ''; 
         addLeaderNameTextbox.value = '';
         addBlockTextbox.value = '';
         addMilMightTextbox.value = '';
         addContinentTextbox.value = ''; 
     }) 
         .catch(error => document.getElementById('errorDB').innerHTML = error.toString());
} 

function deleteCountry(id) {
     fetch(`${uri}/${id}`, {
         method: 'DELETE' 
     }) 
         .then(() => getCountries()) 
         .catch(error => document.getElementById('errorDB').innerHTML = error.toString());
} 

function displayEditForm(id) {
     const country = countries.find(countries => countries.id === id);
    
    document.getElementById('edit-Id').value = country.id;
    document.getElementById('edit-Name').value = country.countryName;
    document.getElementById('edit-LeaderName').value = country.leaderName;
    document.getElementById('edit-Block').value = country.block;
    document.getElementById('edit-MilMight').value = country.milMight;
    document.getElementById('edit-Continent').value = country.continentsIds;
    document.getElementById('editCountry').style.display = 'block'; 
}
    function updateCountry() { 
    // метод PUT 
        const countryId = document.getElementById('edit-Id').value; 
        const country = {
            id: parseInt(countryId, 10),
            countryName: document.getElementById('edit-Name').value.trim(), 
            leaderName: document.getElementById('edit-LeaderName').value.trim(),
            block: document.getElementById('edit-Block').value.trim(), 
            milMight: document.getElementById('edit-MilMight').value.trim(), 
            continentsIds: document.getElementById('edit-Continent')
                .value.replaceAll(" ", "")
                .split(",") 
                .map(str => {
                    return Number(str) 
                })
        }
         fetch(`${uri}/${countryId}`, {
             method: 'PUT', 
             headers: { 'Accept': 'application/json',
                 'Content-Type': 'application/json' 
             }, 
             body: JSON.stringify(country) 
         }) 
             .then(response => { 
                 if(!response.ok){
                    return response.text().then(text => { 
                        throw new Error(text) 
                    })} 
                 document.getElementById('errorDB').innerHTML = "";
                 return getCountries();}) 
             .then(() => getCountries())
             .catch(error => document.getElementById('errorDB').innerHTML =
                 error.toString());
        
        closeInput(); 
        
         return false; 
} 

function closeInput() {
     document.getElementById('editCountry').style.display = 'none'; 
     document.getElementById('errorDB').innerHTML=''; 
} 

function _displayCountries(data) {
    const tBody = document.getElementById('countries'); 
     tBody.innerHTML = '';
     
     const button = document.createElement('button');  
     data.forEach(country => { 
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Редагувати';
        editButton.setAttribute('onclick',
            `displayEditForm(${country.id})`);
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Видалити';
        deleteButton.setAttribute('onclick', `deleteCountry(${country.id})`);
        let tr = tBody.insertRow();
 
         let td0 = tr.insertCell(0); 
         let textNodeId = document.createTextNode(country.id);
         td0.appendChild(textNodeId); 
         let td1 = tr.insertCell(1); 
         let textNodeCountryName = document.createTextNode(country.countryName);
         td1.appendChild(textNodeCountryName); 
         let td2 = tr.insertCell(2); 
         let textNodeLeaderName = document.createTextNode(country.leaderName); 
         td2.appendChild(textNodeLeaderName); 
         let td3 = tr.insertCell(3); 
         let textNodeBlock = document.createTextNode(country.block); 
         td3.appendChild(textNodeBlock);
         let td20 = tr.insertCell(4);
         let textNodeMilMight = document.createTextNode(country.milMight);
         td20.appendChild(textNodeMilMight);
         let td30 = tr.insertCell(5);
         let textNodeContinents = document.createTextNode(country.continentsIds.join(" "));
         td30.appendChild(textNodeContinents);
         let td4 = tr.insertCell(6); 
         td4.appendChild(editButton);
         let td5 = tr.insertCell(7); 
         td5.appendChild(deleteButton); 
     }); 
     countries = data; 
 }