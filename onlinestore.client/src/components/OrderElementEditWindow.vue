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

        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    }
}
</script>
<template></template>
<style scoped></style>