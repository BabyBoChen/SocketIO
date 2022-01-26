<template>
    <div class="inputbox-wrapper">
        <textarea ref="txt" @input="onInput($event.target)" v-model="this.$data.text" :placeholder="this.$props.placeHolder+'說:'"></textarea>
        <button class="btn-send" @click="submit()"><i class="fas fa-paper-plane fa-2x"></i></button>
    </div>
</template>

<script>
export default {
    name: 'InputBox',
    props:{
        placeHolder:String,
    },
    data(){
        let model = {
            text : '',
        };
        return model;
    },
    methods:{
        /** @param el {HTMLTextAreaElement} */
        onInput(el){            
            el.style.height = '0rem';
            let newH = this.convertPixelsToRem(el.scrollHeight);
            if(newH <= 3.75){
                el.style.height = '3.75rem';
            } else if (newH >= 5.25){
                el.style.height = '5.25rem';
            } else {
                el.style.height = newH + 'rem';
            }
        },
        submit(){
            /** @type  {HTMLTextAreaElement}*/
            let txt = this.$refs.txt
            let message = txt.value;
            if(message){
                this.$emit('submit',message);
            }
            this.$data.text = '';
            txt.style.height = '3.75rem';
        },
        convertRemToPixels(rem) {    
            return rem * parseFloat(getComputedStyle(document.documentElement).fontSize);
        },
        convertPixelsToRem(pixel){
            return pixel / parseFloat(getComputedStyle(document.documentElement).fontSize);
        }
    },
}
</script>

<style scoped>
.inputbox-wrapper{
    display: flex;
    width: 100%;
    align-items: flex-end;
}
textarea{
    height:3.75rem;
    width:100%;
    resize: none;
}
.btn-send{
    margin-left: 5px;
    height: 3.75rem;
    white-space: nowrap;
}
</style>