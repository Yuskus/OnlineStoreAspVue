<template>
    <div>
        <label>{{ labelName }}</label>
        <input :type="inputType" :value="inputText" :placeholder="placeholderText" :required="isRequired" @input="updateValue($event.target.value)"  />
    </div>
</template>

<script>
export default {
    props: {
        placeholderText: {
            type: String
        },
        isRequired: {
            type: Boolean,
            default: false
        },
        labelName: {
            type: String,
            required: true
        },
        inputType: {
            type: String,
            default: 'text'
        },
        inputText: {}
    },
    methods: {
        updateValue(value) {
            this.$emit('update:modelValue', value);
        }
    }
}
</script>

<style scoped>
  label {
    display: block;
    line-height: 40px;
    margin-top: 20px;
    color: #1c2633;
    text-shadow: 1px 1px rgba(0,0,0,0.1);
    font-weight: 500;
    font-size: 16px;
  }

  input {
    display: block;
    width: 100%;
    min-height: 40px;
    justify-self: center;
    border: none;
    border-radius: 8px;
    outline: 1px solid rgba(28,38,51,0.3);
    padding: 4px;
  }
</style>