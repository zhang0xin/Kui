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
    props: ['path'],
    data: function(){
        return {
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
                <input class='input' type='text' placeholder='标题' />
            </div>
        </div>
        <div class='field'>
            <label class='label'>描述</label>
            <div class='control'>
                <textarea class='textarea' placeholder='描述' />
            </div>
        </div>
        <div class='field is-grouped'>
            <div class='control'>
                <button class='button is-primary'>保存</button>
            </div>
            <div class='control'>
                <button class='button'>取消</button>
            </div>
        </div>
    </div>
    `
})