<script>
import axios from 'axios';

export default {
    props: {
        order: {
            type: Object
        }
    },
    data() {
        return {
            localOrder: { ...this.order }
        }
    },
    methods: {
        async applyChanges() {
            try {
                // to leave only update and delete http requests
                const response = await axios.post(`http://localhost:5000/api/orders/add`, this.localOrder, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response.status === 200 && response.data) {
                    // result
                } else {
                    alert('Проблемы с сервером!');
                    console.log("Статус ошибки: " + response.status);
                }
                this.cancelDialog();
            } catch (error) {
                console.error('Ошибка при получении данных (Orders): ', error);
                alert('Проблемы с сервером!');
                this.cancelDialog();
            }
        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    }
}
</script>

<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать товар</h2>
            <div class="form">
                <label>Код</label>
                <input type="text" v-model="localOrder.code">
            </div>
            <div class="form">
                <label>Имя</label>
                <input type="text" v-model="localOrder.name">
            </div>
            <div class="form">
                <label>Категория</label>
                <input type="text" v-model="localOrder.category">
            </div>
            <div class="form">
                <label>Цена</label>
                <input type="number" v-model="localOrder.price">
            </div>
    
            <div class="buttons">
                <button @click="applyChanges()">Изменить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
    </template>

<style scoped></style>