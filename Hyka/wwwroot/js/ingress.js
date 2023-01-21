'use strict';

const modal = document.getElementById('registerModal');
const btn_modal = document.getElementById('btn_modal');
const main_input = document.getElementById('main_input');
const modal_input = document.getElementById('modal_input');

btn_modal.addEventListener('click', () => {
    getAllCountries();
    getAllDistricts();
});

main_input.addEventListener('input', () => {
    validateInputData('main_input');
});


modal_input.addEventListener('input', () => {
    validateInputData('modal_input');
});


function getFlagEmoji(countryCode) {
    const codePoints = countryCode
        .toUpperCase()
        .split('')
        .map(char => 127397 + char.charCodeAt());
    return String.fromCodePoint(...codePoints);
}

async function getAllCountries() {
    const URL = 'https://restcountries.com/v2/all'
    const selector = document.querySelector("#Country")
    const response = await fetch(URL);
    const COUNTRIES = await response.json();
    let data = '<option value=""></option>';
    for (const country of COUNTRIES) {
        let emoji = getFlagEmoji(country.alpha3Code);
        let name = country.translations.es
        data += `<option value="${name.toUpperCase()}"> ${name} ${emoji}</option>`
    }
    selector.innerHTML = data
    // Colombia is the default country
    selector.selectedIndex = 52;
}

async function getAllDistricts() {
    const PATH = '/js/districts.json';
    const selector = document.querySelector('#District')
    const response = await fetch(PATH);
    const DISTRICTS = await response.json();
    let data = '<option value=""></option>';
    for (const district of DISTRICTS) {
        data += `<option value="${district.name.toUpperCase()}"> ${district.name}</option>`
    }
    selector.innerHTML = data
}

// Validate ingress form
function validateInputData(inputId) {
    const CARD = {
        'CardId': document.getElementById('CardId'),
        'Id': document.getElementById('Id'),
        'FullName': document.getElementById('FullName')
    }
    const element = document.getElementById(inputId);
    const data = element.value;
    element.type = 'password';
    try {
        const regex = /^\d+$/;
        if ((regex.exec(data)) !== null) {
            element.type = 'text';
        }
        const object = JSON.parse(data);
        if (typeof (object) === 'object' && Object.keys(object).length == 3) {
            element.value = '';
            $("#registerModal").modal("show");
            $("#collapseOne").collapse("hide");
            if (CARD['Id'].value == object.documento) {
                CARD['CardId'].value = object.codigo;
            }
            if ((CARD['CardId'].value == '' && CARD['Id'].value == '' && CARD['FullName'].value == '') ||
                document.getElementById('BloodType').value == '') {
                CARD['CardId'].value = object.codigo;
                CARD['Id'].value = object.documento;
                CARD['FullName'].value = object.nombre.toUpperCase();
            }
        }
    } catch (error) {
        console.log(error);
    }
}

function getDataFromObject(encondeDto) {
    const SELECTORS = {
        'DocumentType': document.getElementById('DocumentType'),
        'Gender': document.getElementById('Gender'),
        'Country': document.getElementById('Country'),
        'District': document.getElementById('District')
    };
    try {
        $("#registerModal").modal("show");
        const object = JSON.parse(atob(encondeDto));
        Object.entries(object).forEach(([key, value]) => {
            if (SELECTORS[key]) {
                const element = SELECTORS[key]
                if (value != null)
                    element.innerHTML = `<option value="${value}">${value}</option>`
                if (key == 'District' && value == null)
                    getAllDistricts();
                if (key == 'Country' && value == null)
                    getAllCountries();
            } else {
                const element = document.getElementById(key);
                element.value = value;
                element.setAttribute('readonly', true);
            }
        });
        modal.addEventListener('hidden.bs.modal', () => {
            location.reload();
        });
    } catch (error) {
        console.log(error);
    }
}
