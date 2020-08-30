const uri = 'api/ThiSinhs/';
new Vue({
    el: '#app',
    data: {
        thisinh: null,
        cmnd: '',
        diachi_nhanGB: '',
        Image:'',
        error: '',
        errors: '',
        img1Src:''
    },
    methods: {
        getInformation: function (event) {
            axios.get(uri + this.cmnd)
                .then(response => {
                    this.thisinh = response.data;
                })
                .catch(() => this.error = true);


        },
        saveInfor: async function (event) {
            await this.uploadImage();
            axios.put(uri, {
                CMND: this.cmnd,
                DiaChi: this.diachi_nhanGB,
                Image: this.Image
            }).then(response => { alert(response.data); })
                .catch(e => {
                    this.errors.push(e)
                })
        },
        uploadImage: function () {
            const selectedImage = document
                .querySelector('input[type=file]')
                .files[0]; // get first file
            this.createBase64Image(selectedImage);
        },
        createBase64Image: function(fileObject) {
            const reader = new FileReader();
            reader.onload = (e) => {
                this.Image = e.target.result;
                alert("xxxxxxxxxxxxxx"+this.Image);
            };

            reader.readAsDataURL(fileObject);
        }

    }
});
