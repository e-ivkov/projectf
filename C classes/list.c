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

/* Function to add a node at the beginning of Linked List. 
   This function expects a pointer to the data to be added 
   and size of the data type */
void push(struct Node **head_ref, void *new_data, size_t data_size)
{
    // Allocate memory for node
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));

    new_node->data = malloc(data_size);
    new_node->next = (*head_ref);

    // Copy contents of new_data to newly allocated memory.
    // Assumption: char takes 1 byte.
    int i;
    for (i = 0; i < data_size; i++)
        *(char *)(new_node->data + i) = *(char *)(new_data + i);

    // Change head pointer as new node is added at the beginning
    (*head_ref) = new_node;
}

/* Function to print nodes in a given linked list. fpitr is used 
   to access the function to be used for printing current node data. 
   Note that different data types need different specifier in printf() */
void printList(struct Node *node, void (*fptr)(void *))
{
    while (node != NULL)
    {
        (*fptr)(node->data);
        node = node->next;
    }
}

struct Node *createEmptyList()
{
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));
    new_node->next = NULL;
    new_node->data = NULL;
    new_node->index = -1;
    return new_node;
}

void put(struct Node *head, void *element)
{
    struct Node *new_node = (struct Node *)malloc(sizeof(struct Node));
    new_node->data = element;
    new_node->next = NULL;
    if (head->next == NULL)
    {
        new_node->index = 0;
    }
    else
    {
        struct Node *node = head;
        while (node->next != NULL)
        {
            node = node->next;
        }
        node->next = new_node;
        new_node->index = node->index + 1;
    }
    
    
}

main(int argc, char const *argv[])
{
    struct Node *head = createEmptyList();
    return 0;
}
