<template>
    <div class="container">
        <div v-if="isBlocked" class="regulator-disabled box">-</div>
        <div v-else class="regulator box" @click="minus()">-</div>
        <div class="quantity box">{{ modelValue }}</div>
        <div class="regulator box" @click="plus()">+</div>
    </div>
</template>

<script>
export default {
    props: [ 'modelValue', 'index' ],
    emits: [ 'update:modelValue', 'update-request' ],
    data() {
        return {
            isBlocked: false
        }
    },
    methods: {
        plus() {
            this.isBlocked = false;
            this.$emit('update:modelValue', this.modelValue + 1);
            this.$emit('update-request', this.index);
        },
        minus() {
            const newValue = this.modelValue - 1;
            if (newValue === 1) {
                this.isBlocked = true;
            }
            this.$emit('update:modelValue', newValue);
            this.$emit('update-request', this.index);
        }
    }
}
</script>

<style scoped>
.container {
    display: flex;
    width: 90px;
    border: 1px solid rgb(137, 163, 211);
}

.box {
    height: 30px;
    width: 30px;
    line-height: 30px;
    text-align: center;
}

.regulator-disabled {
    background-color: rgb(137, 163, 211);
}

.regulator {
    background-color: cornflowerblue;
}

.regulator:hover {
    background-color: rgb(125, 163, 233);
}

.regulator:active {
    background-color: rgb(137, 163, 211);
}

.quantity {
    background-color: white;
}
</style>