import axios from 'axios';
const uri = 'api/ThiSinhs';
const axios = require('axios');

async function makeRequest() {

    const config = {
        method: 'get',
        url:  uri + '/' + '0123369963'
    }

    let res = await axios(config)

    console.log(res.status);
    alert(res.status);
}



function encodeImgtoBase64() {

    var img = document.getElementById('img').files[0];;
    var imgBase64 = "";
    var reader = new FileReader();

    reader.onloadend = function () {

        imgBase64 = reader.result;
    }
    reader.readAsDataURL(img.name);
    return imgBase64;
}