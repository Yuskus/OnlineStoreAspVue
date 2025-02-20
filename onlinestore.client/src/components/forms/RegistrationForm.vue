<template>
    <div>
        <AuthLineForm v-model="modelValue.username" @update:modelValue="updateModel" labelName="Введите логин:" inputType="text" placeholderText="Логин" :isRequired="true" autocomplete="new-password" />
        <AuthLineForm v-model="modelValue.password" @update:modelValue="updateModel" labelName="Введите пароль:" inputType="password" placeholderText="Пароль" :isRequired="true" autocomplete="new-password" />
        
        <div v-if="isCustomer">
            <AuthLineForm v-model="modelValue.customerInfo.name" @update:modelValue="updateModel" labelName="Введите имя заказчика:" inputType="text" placeholderText="Имя заказчика" :isRequired="true" autocomplete="new-password" />
            <AuthLineForm v-model="modelValue.customerInfo.code" @update:modelValue="updateModel" labelName="Введите код заказчика:" inputType="text" placeholderText="1234-2000" :isRequired="true" autocomplete="new-password" />
            <AuthLineForm v-model="modelValue.customerInfo.address" @update:modelValue="updateModel" labelName="Введите адрес заказчика:" inputType="text" placeholderText="Адрес заказчика" :isRequired="false" autocomplete="new-password" />
        </div>
    </div>
</template>

<script>
import AuthLineForm from './AuthLineForm.vue';
export default {
    components: { AuthLineForm },
    props: {
        modelValue: {
            type: Object,
            default: {
                customerInfo: {
                    name: "",
                    code: "",
                    address: null
                },
                username: "",
                password: ""
            }
        },
        isCustomer: {
            type: Boolean,
            default: true
        }
    },
    emits: [ 'update:modelValue' ],
    methods: {
        updateModel() {
            this.$emit('update:modelValue', this.modelValue);
        }
    }
}
</script>

<style scoped>
</style>
