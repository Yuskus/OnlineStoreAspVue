<template>
    <div>
        <hr>
        <div class="form">
            <label>{{ this.labelName }}</label>
            <input :type="inputType" :value="modelValue" @input="updateValue($event.target.value)" />
        </div>
    </div>
</template>

<script>
export default {
    props: [
        'labelName',
        'inputType',
        'modelValue'
    ],
    emits: ['update:modelValue'],
    methods: {
        updateValue(value) {
            this.$emit('update:modelValue', value);
        }
    }
}
</script>

<style scoped>
.form {
    display: flex;
    margin-bottom: 15px;
}

.form label {
    width: 50%;
    margin-right: 10px;
    padding: 10px;
}

.form input {
    width: 50%;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid rgba(0,0,80,0.5);
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>