<template>
    <div>
        <hr>
        <div class="form">
            <label>{{ labelName }}</label>
            <select v-model="selected" @change="updateValue(selected)">
                <option v-for="option in options" :key="option" :value="option">{{ option }}</option>
            </select>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        options: {
            type: Array,
            required: true
        },
        labelName: {
            type: String,
            required: true
        },
        modelValue: {
            type: String
        }
    },
    data() {
        return {
            selected: this.modelValue
        }
    },
    methods: {
        updateValue(value) {
            this.$emit('update:modelValue', value);
        }
    },
    watch: {
        modelValue(newValue) {
            this.selected = newValue;
        }
    }
}
</script>

<style scoped>
.form {
    display: flex;
    margin-bottom: 15px;
}

label {
    width: 50%;
    margin-right: 10px;
    padding: 10px;
}

select {
    width: 50%;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid rgba(0,0,80,0.5);
}

option {
    font-size: 16px;
    background-color: white;
}

option:checked {
    font-weight: 700;
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>