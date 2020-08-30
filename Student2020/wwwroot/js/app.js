const uri = 'api/ThiSinhs/';
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
                })
                .catch(() => this.error = true);
        },
        submit: function () {
            axios.put(uri, this.form)
                .then(response => { alert(response.data); })
                .catch(e => {
                    this.errors.push(e)
                })
        },
        getImage: function (e) {
            const reader = new FileReader();
            reader.onload = (x) => {
                this.form.imageData = x.target.result;
            }

            const file = e.target.files[0];
            this.form.imageFileName = file.name;

            reader.readAsDataURL(file);
        }
    }
});
