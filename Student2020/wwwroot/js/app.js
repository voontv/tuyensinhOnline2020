const uri = 'http://27.71.235.200:801/api/ThiSinhs/';
new Vue({
    el: '#app',
    data: {
        thisinh: null,
        form: {
            cmnd: ""
        },
        error: ''
    },
    methods: {
        getInformation: function () {
            axios.get(uri + this.form.cmnd)
                .then(response => {
                    this.thisinh = response.data;
                    this.form.DiaChi = this.thisinh.diaChi;
                })
                .catch(() => this.error = true);
        },
        submit: function () {
            axios.put(uri, this.form)
                .then(() => {
                    this.getInformation();
                })
                .catch(e => {
                    console.log(e);
                    this.errors.push(e);
                })
        },
        getDownloadUrl(file){
          return uri + "download/"+ file;
        },
        getImage: function (e) {
            const reader = new FileReader();
            reader.onload = (x) => {
                this.form.imageData = x.target.result;
            }

            const file = e.target.files[0];
            this.form.imageFileName = file.name;

            reader.readAsDataURL(file);
        },
        getStatusImage: function (status){
            if(!!status){
                return "/img/yes.png";
            }else{
                return "/img/no.png";
            }
        }
    }
});
