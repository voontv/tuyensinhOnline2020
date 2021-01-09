moment.locale('vi');

const uri = 'api/ThiSinhs/';
new Vue({
    el: '#app',
    data: {
        thisinh: null,
        form: {
            cmnd: "",
            nganhChon: null
        },
        error: ''
    },
    filters: {
        dtFormat(dt) {
            return moment.utc(dt).format('L');
        }
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
            if (this.form.nganhChon == null && this.thisinh.maNganh2 != null && this.thisinh.maChon == null)
            {
                alert("Anh/Chị cần chọn 1 ngành học, không được để trống.");
                return true;
            }
            axios.put(uri, this.form)
                .then(() => {
                    this.getInformation();
                    alert("Cập nhật thành công.");
                })
                .catch(e => {
                    console.log(e);
                    this.errors.push(e);
                })
        },
        getDownloadUrl(file) {
            return uri + "download/" + file + "?cmnd=" + this.thisinh.cmnd;
        },
        getDocumentDownloadUrl() {
            return uri + "download-pdf/" + this.thisinh.cmndImg + "?cmnd=" + this.thisinh.cmnd;
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
        },
        formatPrice: function (value) {
            let val = (value / 1)
            return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        }
    }
});
