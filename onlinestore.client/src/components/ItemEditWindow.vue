<script>
import axios from 'axios';

export default {
    props: {
        item: {
            type: Object
        }
    },
    data() {
        return {
            localItem: { ...this.item },
            idOfItem: null
        }
    },
    methods: {
        async addItem() {
            try {
                const response = await axios.post(`http://localhost:5000/api/items/add`, this.localItem, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response.status === 200 && response.data) {
                    console.log("Добавлено");
                } else {
                    alert('Проблемы с сервером!');
                    console.log("Статус ошибки: " + response.status);
                }
                this.cancelDialog();
            } catch (error) {
                console.error('Ошибка при получении данных (ItemEdit): ', error);
                alert('Проблемы с сервером!');
                this.cancelDialog();
            }
        },
        async applyChanges() {
            try {
                const response = await axios.put(`http://localhost:5000/api/items/update/${this.localItem.id}`, {
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
                console.error('Ошибка при получении данных (ItemEdit): ', error);
                alert('Проблемы с сервером!');
                this.cancelDialog();
            }
        },
        async deleteItem() {
            try {
                const response = await axios.delete(`http://localhost:5000/api/items/delete/${this.localItem.id}`, {
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
                console.error('Ошибка при получении данных (ItemEdit): ', error);
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
            <input type="text" v-model="localItem.code">
        </div>
        <div class="form">
            <label>Имя</label>
            <input type="text" v-model="localItem.name">
        </div>
        <div class="form">
            <label>Категория</label>
            <input type="text" v-model="localItem.category">
        </div>
        <div class="form">
            <label>Цена</label>
            <input type="number" v-model="localItem.price">
        </div>

        <div class="buttons">
            <button @click="applyChanges()">Изменить</button>
            <button @click="deleteItem()">Удалить</button>
            <button @click="cancelDialog()">Отмена</button>
        </div>
    </div>
</div>
</template>

<style scoped>
.dialog-over {
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
}

.dialog {
    background-color: white;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.buttons {
    background-color: blue;
}

.form {
    color: gray;
}
</style>