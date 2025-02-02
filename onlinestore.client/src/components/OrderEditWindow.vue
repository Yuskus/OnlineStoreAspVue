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
        async deleteItem() {
            try {
                const response = await axios.delete(`http://localhost:5000/api/orders/delete/${this.localOrder.id}`, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response.status === 200 && response.data) {
                    console.log("Удалено");
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
        async applyChanges() {
            try {
                const response = await axios.put(`http://localhost:5000/api/orders/update/${this.localOrder.id}`, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response.status === 200 && response.data) {
                    console.log("Обновлено");
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
    <!--изменить лейблы и инпуты-->
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
                <button @click="deleteItem()">Удалить</button>
                <button @click="applyChanges()">Изменить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
    </template>

<style scoped>
/*написать стили*/
</style>