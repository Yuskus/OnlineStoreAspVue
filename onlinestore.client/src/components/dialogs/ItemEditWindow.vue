<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать товар</h2>

            <DialogLineForm v-model="localItem.code" :labelName="'Код'" :inputText="localItem.code" />
            <DialogLineForm v-model="localItem.name" :labelName="'Имя'" :inputText="localItem.name" />
            <DialogLineForm v-model="localItem.category" :labelName="'Категория'" :inputText="localItem.category" />
            <DialogLineForm v-model="localItem.price" :labelName="'Цена'" :inputType="'number'" :inputText="localItem.price" />
    
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
import { addItem, updateItem, deleteItem } from '../../api/itemsApi';

import DialogLineForm from '../forms/DialogLineForm.vue';

export default {
    components: { DialogLineForm },
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
        makeItemRequestBody() {
            return {
                code: this.localItem.code,
                name: this.localItem.name,
                price: this.localItem.price,
                category: this.localItem.category
            };
        },
        async addItem() {
            if (this.localItem.code === this.item.code) {
                alert("Для добавления нового товара нужно изменить его код!");
                return;
            }
            try {
                let newItem = this.makeItemRequestBody();
                const response = await addItem(newItem);
                if (!response) {
                    alert('Ошибка при добавлении товара (Item).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при добавлении данных (ItemEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async applyChanges() {
            try {
                let newItem = this.makeItemRequestBody();
                const response = await updateItem(this.localItem.id, newItem);
                if (!response) {
                    alert('Ошибка при изменении товара (Item).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при изменении данных (ItemEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async deleteItem() {
            try {
                const response = await deleteItem(this.localItem.id);
                if (!response) {
                    alert('Ошибка при удалении товара (Item).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (ItemEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert('Проблемы с сервером!');
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