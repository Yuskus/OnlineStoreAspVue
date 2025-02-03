<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать товар</h2>
            <hr>
            <div class="form">
                <label>Код</label>
                <input type="text" v-model="localItem.code">
            </div>
            <hr>
            <div class="form">
                <label>Имя</label>
                <input type="text" v-model="localItem.name">
            </div>
            <hr>
            <div class="form">
                <label>Категория</label>
                <input type="text" v-model="localItem.category">
            </div>
            <hr>
            <div class="form">
                <label>Цена</label>
                <input type="number" v-model="localItem.price">
            </div>
    
            <div class="buttons">
                <button @click="addItem()">Добавление</button>
                <button @click="applyChanges()">Изменить</button>
                <button @click="deleteItem()">Удалить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
</template>

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
            localItem: { ...this.item }
        }
    },
    methods: {
        async addItem() {
            try {
                if (this.localItem.code === this.item.code) {
                    alert("Для добавления нового товара нужно изменить его код!");
                }
                const response = await axios.post(`http://localhost:5000/api/items/add`, { 
                    code: this.localItem.code,
                    name: this.localItem.name,
                    category: this.localItem.category,
                    price: this.localItem.price
                }, {
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
                console.log(`http://localhost:5000/api/items/update/${this.localItem.id}`);
                const response = await axios.put(`http://localhost:5000/api/items/update/${this.localItem.id}`, this.localItem, {
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

<style scoped>
.dialog-over {
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 0px;
    bottom: 0px;
    left: 0px;
    right: 0px;
    transform: scale(1.25);
    background-color: rgba(0,0,0,0.4);
    z-index: 1000;
}

.dialog {
    background-color: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
    min-width: 400px;
    width: 50vw;
    min-height: fit-content;
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

h2 {
    color: #212933;
}

h2, label, button {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-style: normal;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

.buttons button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

.buttons button:hover {
    background-color: rgba(0,0,30,0.25);
}

.buttons button:focus {
    background-color: rgba(0,0,30,0.4);
}

.form {
    display: flex;
    margin-bottom: 15px;
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>