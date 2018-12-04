#include <stdio.h>
#include <stdlib.h>

/* A linked list node */
struct Node
{
    // Any data type can be stored in this node
    void *data;
    int index;
    struct Node *next;
};

/* Function to print nodes in a given linked list. fpitr is used 
   to access the function to be used for printing current node data. 
   Note that different data types need different specifier in printf() */
void printList(struct Node *head) //void printList(struct Node *node, void (*fptr)(void *))
{
    struct Node *node = head->next;
    while (node != NULL)
    {
        printf("%d \n", *(int *)(node->data));
        node = node->next;
    }
}

struct Node *l_createEmptyList()
{
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));
    new_node->next = NULL;
    new_node->data = NULL;
    new_node->index = -1;
    return new_node;
}
void l_put(struct Node *head, void *element)
{
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));
    new_node->data = element;
    new_node->next = NULL;
    if (head->next == NULL)
    {
        new_node->index = 0;
        head->next = new_node;
    }
    else
    {
        struct Node *node = head->next;
        while (node->next != NULL)
        {
            node = node->next;
        }
        node->next = new_node;
        new_node->index = node->index + 1;
    }
}

/*struct Node *l_createList_with_array(void* arr)
{
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));
    new_node->next = NULL;
    new_node->data = NULL;
    new_node->index = -1;
    for (int i = 0; i < sizeof(arr); i = i + (sizeof(*arr)))
    {
        l_put(new_node, arr[i]);
    }
    return new_node;
}
*/

struct Node *l_concatenate(struct Node *first, struct Node *second)
{
    struct Node *node = first->next;
    struct Node *head = l_createEmptyList();
    while (node != NULL)
    {

        l_put(head, (void *)node->data);
        node = node->next;
    }
    node = second->next;
    while (node != NULL)
    {

        l_put(head, (void *)node->data);
        node = node->next;
    }
    return head;
}

int l_length(struct Node *head)
{
    int i = 0;
    struct Node *node = head->next;
    while (node != NULL)
    {
        i++;
        node = node->next;
    }
    return i;
}

void *l_get(struct Node *head, int index)
{
    struct Node *node = head->next;
    while (node != NULL && node->index != index)
    {
        node = node->next;
    }
    if (node != NULL)
    {
        return node->data;
    }
    return NULL;
}

void l_for_loop(struct Node *head)
{
    struct Node *node = head->next;
    while (node != NULL)
    {
        void *data = node->data;

        /*
            here you should cast you data, ex:
            data = (int*)(data);
            now in data your int number

            'some your code here'

            use you data then the last string:
        */
        node = node->next;
    }
}
/*
main(int argc, char const *argv[])
{
    struct Node *head = l_createEmptyList();
    void *ptr = malloc(sizeof(int));
    *((int *)ptr) = 1;
    l_put(head, ptr);
    ptr = malloc(sizeof(int));
    *((int *)ptr) = 2;
    l_put(head, ptr);
    ptr = malloc(sizeof(int));
    *((int *)ptr) = 3;
    l_put(head, ptr);
    printList(head);
    printf("Length: %d \n", l_length(head));

    struct Node *head2 = l_createEmptyList();
    ptr = malloc(sizeof(int));
    *((int *)ptr) = 4;
    l_put(head2, ptr);
    ptr = malloc(sizeof(int));
    *((int *)ptr) = 5;
    l_put(head2, ptr);
    
    printf("\nSecond List:\n");
    printList(head2);
    printf("Length:%d\n", l_length(head2));

    struct Node *result = l_concatenate(head, head2);
    printf("\nConcatenate List:\n");
    printList(result);

    printf("Concatenate Length:%d\n\n", l_length(result));
    int a = *(int*)(l_get(result, 1));    
    printf("A: %d\n\n\n",a);
    printf("GET out of range element: %d", (int *)(l_get(result, 5)));
    return 0;
}
*/