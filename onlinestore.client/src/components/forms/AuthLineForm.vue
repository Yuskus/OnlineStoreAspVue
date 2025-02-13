<template> 
    <div>
        <label>{{ labelName }}</label>
        <input :type="inputType" :placeholder="placeholderText" :required="isRequired" :value="text" @input="updateValue($event.target.value)" />
    </div>
</template>

<script>
export default { // F I X 
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
        }
    },
    data() {
        return {
            text: ''
        }
    },
    methods: {
        updateValue(value) {
            this.text = value;
            this.$emit('update-value', value);
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