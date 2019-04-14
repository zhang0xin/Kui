var api = {
    methods:{
        getSiteNode: function(path){
            axios.get("/Api/GetSiteNode", {
                params: {
                    path: path
                }
            }).then(function(response){

            })
        },
        saveSiteNode: function(node){
            axios.post("/Api/SaveSiteNode", node).then(function(response){
                console.log(response)
            })
        }
    }
}

Vue.component('managePage', {
    props: ['path'],
    template: `
    <div>
        <b>manage page</b>
        <router-link :to="'/edit/'+path">新建</router-link>
    </div>
    `
})
Vue.component('editPage', {
    mixins: [api],
    props: ['path'],
    data: function(){
        return {
            node: {
                title: '',
                content: ''
            }
        }
    },
    methods: {
        save: function(){
            var param = {
                type : 'PageNode',
                parentPath: this.path,
                data : this.node
            }
            this.saveSiteNode(param)
        }
    },
    template: `
    <div>
        <div class='field'>
            <label class='label'>类别</label>
            <div class='control'>
                <input class='input' type='text' placeholder='类别' :value='path' disabled />
            </div>
        </div>
        <div class='field'>
            <label class='label'>标题</label>
            <div class='control'>
                <input v-model='node.title' class='input' type='text' placeholder='标题' />
            </div>
        </div>
        <div class='field'>
            <label class='label'>内容</label>
            <div class='control'>
                <textarea v-model='node.content' class='textarea' placeholder='描述' />
            </div>
        </div>
        <div class='field is-grouped'>
            <div class='control'>
                <button v-on:click='save' class='button is-primary'>保存</button>
            </div>
            <div class='control'>
                <button class='button'>取消</button>
            </div>
        </div>
    </div>
    `
})