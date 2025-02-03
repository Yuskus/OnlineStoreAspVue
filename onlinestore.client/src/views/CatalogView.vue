<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <ItemEditWindow v-if="role === '1' && isOpenDialog === true" @close-dialog="clickWindowRedactor" :item="selectedItem" />

  <div class="filters">
      <div class="button" @click="getItems()">Все категории</div>
      <div class="button" v-for="(category, index) in categories" @click="getByCategory(category)" :key="index">{{ category }}</div>
    </div>

  <div class="container">

    <div v-if="items.length > 0" class="catalog">
      <div class="item" v-for="(item, index) in items" :key="index" @click="clickOnItem(index)" >
        <div><img src="../assets/item.jpg" /></div>
        <div class="item-desc">{{ item.name }}</div>
        <div class="item-desc">{{ item.category }}</div>
        <div class="item-desc">{{ item.price }}</div>
      </div>
    </div>
    <div v-else>
      <h2>Товаров нет.</h2>
    </div>

    <Pagination @page-changed="getItems" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<script>
  import ItemEditWindow from '../components/ItemEditWindow.vue';
  import Pagination from '../components/PaginationComponent.vue';
  import axios from 'axios';

  export default {
    name: 'Catalog',
    components: { Pagination, ItemEditWindow },
    data() {
      return {
        role: '',
        items: [],
        currentPage: 1,
        totalPages: 1,
        totalCount: 0,
        pageSize: 12,
        categories: [],
        selectedCategory: '',
        basketOrder: null,
        selectedItem: null,
        isOpenDialog: false
      }
    },
    methods: {
      getMyData() {
        this.role = localStorage.getItem('role');
      },
      async urlRequestGET(url) {
        try {
          const response = await axios.get(url, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            return response;
          } else {
            alert('Проблемы с сервером!');
            console.log(response.data);
            console.log("Статус ошибки: " + response.status + " 32");
          }
          return null;
        } catch (error) {
          console.error('Ошибка при получении данных (Catalog): ', error);
          alert('Проблемы с сервером!');
        }
        return null;
      },
      async urlRequestPOST(url, body) {
        try {
          const response = await axios.post(url, body, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            return response;
          } else {
            alert('Проблемы с сервером!');
            console.log(response.data);
            console.log("Статус ошибки: " + response.status);
          }
          return null;
        } catch (error) {
          console.error('Ошибка при получении данных (Catalog): ', error);
          alert('Проблемы с сервером!');
        }
        return null;
      },
      async getByCategory(category, number = 1) {
        console.log("getByCategory " + category);
        this.currentPage = number;
        const response = await this.urlRequestGET(`http://localhost:5000/api/Items/category/${category}?pageNumber=${this.currentPage}&pageSize=${this.pageSize}`);
        if (response !== null) {
          this.items = response.data.itemResponses;
          this.totalCount = response.data.totalCount;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        }
      },
      async getCategories() {
        console.log("getCategories");
        const response = await this.urlRequestGET('http://localhost:5000/api/Items/getcategories');
        if (response !== null) {
          this.categories = response.data;
        }
      },
      async getItems(number = 1) {
        console.log("getItems "+number);
        this.currentPage = number;
        const response = await this.urlRequestGET(`http://localhost:5000/api/Items/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}`);
        if (response !== null) {
          this.items = response.data.itemResponses;
          this.totalCount = response.data.totalCount;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        }
      },
      async clickOnItem(index) {
        if (this.role === '1') {
          this.selectedItem = this.items[index];
          this.clickWindowRedactor(true);
        } else {
          await this.addInBasket(this.items[index]); //
        }
      },
      async getBasketNumber() {
        if (this.role === '1') return;

        const response = await this.urlRequestGET(`http://localhost:5000/api/Orders/getbasket/${localStorage.getItem('guid')}`);
        if (response !== null) {
          this.basketOrder = response.data;
        }
      },
      async addInBasket(item) {
        let body = {
          orderId: this.basketOrder.id,
          itemId: item.id,
          itemsCount: 1,
          itemPrice: item.price // минус персональная скидка (потом дописать)
        }
        const response = await this.urlRequestPOST(`http://localhost:5000/api/OrderElements/add`, body);
        if (response !== null) {
          let guid = response.data;
          console.log(guid);
        }
      },
      clickWindowRedactor(state) {
        this.isOpenDialog = state;
      }
    },
    mounted() {
      this.getMyData();
      this.getCategories();
      this.getItems();
      this.getBasketNumber();
    }
  }
</script>

<style scoped>
  .container {
    max-width: 70vw;
    width: fit-content;
    margin: 0px auto;
    padding: 20px 0px 40px 0px;
  }

  .filters {
    display: block;
    max-width: 70vw;
    margin: 0 auto;
    padding: 10px;
    align-items: center;
  }

  .button {
    display: inline-block;
    justify-content: space-around;
    text-decoration: none;
    margin: 5px 10px;
    padding: 10px;
    background-color: #f0f5fa;
    border-radius: 15px;
  }

  a, h1, h2, h3, p, .cell, div {
      font-family: "Sofia Sans", serif;
      font-optical-sizing: auto;
      font-weight: 500;
      font-style: normal;
      color: #1c2633;
  }

  .button:hover {
    background-color: #dce1f0;
  }

  .catalog {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    padding: 20px 0px 30px 0px;
  }

  .item {
    background-color: #f0f5fa;
    align-items: center;
    box-shadow: 1px 2px 4px rgba(0, 50, 100, 0.2);
    align-content: space-around;
    margin: 10px;
    border-radius: 20px 20px 20px 20px;
  }

  img {
    border-radius: 20px 20px 0px 0px;
  }

  .item div {
    align-self: center;
  }

  .item-desc {
    padding: 10px;
  }

  .item:hover {
    box-shadow: 4px 8px 16px rgba(0, 50, 100, 0.4);
  }

  p {
    color: #1c2633;
  }
</style>

