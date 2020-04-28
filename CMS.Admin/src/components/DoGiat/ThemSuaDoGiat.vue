<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập đồ giặt' : 'Thêm mới đồ giặt' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formDoGiat">
                    <v-layout row wrap>
                        <v-flex xs12>
                            <v-autocomplete v-model="doGiat.LoaiDoGiatID"
                                            :items="dsLoaiDoGiat"
                                            item-text="TenLoaiDoGiat"
                                            item-value="LoaiDoGiatID"
                                            label="Loại đồ giặt"
                                            :error-messages="errors.collect('Loại đồ giặt', 'formDoGiat')"
                                            v-validate="'required'"
                                            data-vv-scope="formDoGiat"
                                            data-vv-name="Loại đồ giặt"
                                            required></v-autocomplete>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="doGiat.TenDoGiat"
                                          label="Tên đồ giặt"
                                          :counter="50"
                                          :error-messages="errors.collect('Tên đồ giặt', 'formDoGiat')"
                                          v-validate="'required|max:50'"
                                          data-vv-scope="formDoGiat"
                                          data-vv-name="Tên đồ giặt">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="doGiat.PCS"
                                          label="PCS"
                                          :counter="50"
                                          :error-messages="errors.collect('PCS', 'formDoGiat')"
                                          data-vv-scope="formDoGiat"
                                          data-vv-name="PCS">
                            </v-text-field>
                        </v-flex>
                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text @click.native="hide">Hủy</v-btn>
                <v-btn text @click.native="save" color="teal lighten-2"
                       :loading="loadingSave">Lưu</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import DoGiatApi, { DoGiatApiSearchParams } from '@/apiResources/DoGiatApi';
    import { DoGiat } from '@/models/DoGiat';
    import LoaiDoGiatApi, { LoaiDoGiatApiSearchParams } from '@/apiResources/LoaiDoGiatApi';
    import { LoaiDoGiat } from '@/models/LoaiDoGiat';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                dsLoaiDoGiat: [] as LoaiDoGiat[],
                isUpdate: false,
                doGiat: {} as DoGiat,
                valid: false,
                isShow: false,
                loading: false,
                loadingSave: false,
                doGiatID: 0,
                searchParamsDoGiat: {} as DoGiatApiSearchParams,
                searchParamsLoaiDoGiat: { includeEntities: true, rowsPerPage: 10 } as LoaiDoGiatApiSearchParams,
            }
        },
        watch: {
            isShow() {
                this.$validator.errors.clear();
            }
        },
        created() {
            this.getDanhSachLoaiDoGiat();
        },
        methods: {
            hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.doGiatID = item.DoGiatID;
                    this.getDataFromApi(this.doGiatID);
                }
                else {
                    this.doGiat = {} as DoGiat;
                }
            },
            getDanhSachLoaiDoGiat(): void {
                LoaiDoGiatApi.search(this.searchParamsLoaiDoGiat).then(res => {
                    this.dsLoaiDoGiat = res.Data;
                });
            },
            getDataFromApi(id: number): void {
                DoGiatApi.detail(id).then(res => {
                    this.doGiat = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formDoGiat').then((res) => {
                    if (res) {
                        this.doGiat.LoaiDoGiat = undefined;
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            DoGiatApi.update(this.doGiatID, this.doGiat).then(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            DoGiatApi.insert(this.doGiat).then(res => {
                                this.doGiat = res;
                                this.isUpdate = true;
                                this.loadingSave = false;
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>

