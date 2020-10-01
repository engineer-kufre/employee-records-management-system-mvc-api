let myChart1 = document.getElementById("myChart").getContext('2d');

let chart1 = new Chart(myChart1, {
    type: 'doughnut',
    data: {
        labels: ['YES', 'YES BUT IN GREEN'],
        datasets: [{
            data: [69, 31],
            backgroundColor: ['#49A9EA', '#36CAAB']
        }]
    },
    options: {
        title: {
            text: "Do you like doughnuts?",
            display: true
        }
    }
});

getAllEmployees();

function getAllEmployees() {
    let i = 1;
    let employeeArray = [];
    while (i < 3) {
        let url = "https://localhost:44361/employee/allemployees?page=" + i;
        let result = getData(url);
        employeeArray.concat(result);
        i++;
    }
    return employeeArray;
}



async function getData(url) {
    return fetch(url)
        .then(res => res.json())
        .then(returnedUsers => { return returnedUsers; })
        .catch(err => { console.log(err) });
}